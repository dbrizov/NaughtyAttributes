using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(object), true)]
	public class NaughtyPropertyDrawer : PropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (PropertyUtility.GetTargetObjectWithProperty(property) != null)
			{
				if (!PropertyUtility.IsVisible(property))
					return 0;
			}

			var handler = _ScriptAttributeUtility_GetPropertyHandler(property);

			return _PropertyHandler_GetHeight(handler, property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{		
			if (PropertyUtility.GetTargetObjectWithProperty(property) == null)
				DefaultOnGUI(position, property, label, true);
			else
				NaughtyEditorGUI.PropertyField_Implementation(position, property, true, DefaultOnGUI);
		}

		static void DefaultOnGUI(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
		{
			var handler = _ScriptAttributeUtility_GetPropertyHandler(property);

			_PropertyHandler_OnGUI(handler, position, property, label, includeChildren);
		}


		const BindingFlags kAnyStaticMemberBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
		const BindingFlags kAnyInstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

		// UnityEditor.PropertyHandler is an internal type implementing default logic for many editor GUI operations
		private static Type _PropertyHandlerType;
		
		// UnityEditor.ScriptAttributeUtility allows us to retreive an instance of UnityEditor.PropertyHandler for a SerializedProperty
		// This is what Unity calls
		private static Type _ScriptAttributeUtilityType;

		// This method returns an instance of PropertyHandler for a SerializedProperty
		private static Func<SerializedProperty, object> _ScriptAttributeUtility_GetPropertyHandler;
		
		private static Func<object, SerializedProperty, GUIContent, float> _PropertyHandler_GetHeight;
		private static Func<object, Rect, SerializedProperty, GUIContent, bool, bool> _PropertyHandler_OnGUI;

		static NaughtyPropertyDrawer()
		{
			ResolveInternalUnityEditorTypes();
			ResolveInternalUnityEditorMethods();
			OverrideDefaultPropertyHandler();
		}

		static void ResolveInternalUnityEditorTypes()
		{
			var unityEditorAssemblyTypes = typeof(UnityEditor.EditorGUI).Assembly.GetTypes();

			_PropertyHandlerType = unityEditorAssemblyTypes.First(t => t.FullName == "UnityEditor.PropertyHandler");
			_ScriptAttributeUtilityType = unityEditorAssemblyTypes.First(t => t.FullName == "UnityEditor.ScriptAttributeUtility");
		}

		static void ResolveInternalUnityEditorMethods()
		{
			_ScriptAttributeUtility_GetPropertyHandler = (Func<SerializedProperty, object>)
				_ScriptAttributeUtilityType
					.GetMethod("GetHandler", kAnyStaticMemberBindingFlags)
					.CreateDelegate(typeof(Func<SerializedProperty, object>), null);

			Compile_PropertyHandler_GetHeight();
			Compile_PropertyHandler_OnGUI();
		}

		static void OverrideDefaultPropertyHandler()
		{
			// For any type without an explicit property drawer, unity falls back to the '' PropertyHandler.
			// PropertyHandler stores reference to a PropertyDrawer, which in the default handler is null.
			// Overriding the default handler's drawer will make unity use our customised drawer for all types,
			// without needing to attribute the type.
			 
			var ScriptAttributeUtility_sharedNullHandlerField = _ScriptAttributeUtilityType.GetField("s_SharedNullHandler", kAnyStaticMemberBindingFlags);
			
			var sharedNullHandler = ScriptAttributeUtility_sharedNullHandlerField.GetValue(null);

			#if UNITY_2021
			// In 2021, the handler uses a collection of drawers instead of a single drawer
			// ScriptAttributeUtility.s_SharedNullHandler.m_PropertyDrawers
			var PropertyHandler_m_PropertyDrawersField = _PropertyHandlerType.GetField("m_PropertyDrawers", kAnyInstanceMemberBindingFlags);

			var propertyDrawers = new List<PropertyDrawer> { new NaughtyPropertyDrawer() };

			PropertyHandler_m_PropertyDrawersField.SetValue(sharedNullHandler, propertyDrawers);
			#else
			// ScriptAttributeUtility.s_SharedNullHandler.m_PropertyDrawer
			var PropertyHandler_m_PropertyDrawerField = _PropertyHandlerType.GetField("m_PropertyDrawer", kAnyInstanceMemberBindingFlags);

			PropertyHandler_m_PropertyDrawerField.SetValue(sharedNullHandler, new NaughtyPropertyDrawer());
			#endif

		}

		static void Compile_PropertyHandler_GetHeight()
		{
			var paramHandler = Expression.Parameter(typeof(object));
			var paramProperty = Expression.Parameter(typeof(SerializedProperty));
			var paramLabel = Expression.Parameter(typeof(GUIContent));

			var method =
				_PropertyHandlerType
					.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.First(m => m.Name == "GetHeight" && m.GetParameters().Length == 3);

			_PropertyHandler_GetHeight =
				Expression.Lambda<Func<object, SerializedProperty, GUIContent, float>>(
					Expression.Call(
						Expression.TypeAs(paramHandler, _PropertyHandlerType), method,
						paramProperty,
						paramLabel,
						Expression.Constant(true)),
					new[] { paramHandler, paramProperty, paramLabel })
					.Compile();
		}

		static void Compile_PropertyHandler_OnGUI()
		{
			var paramHandler = Expression.Parameter(typeof(object));
			var paramPosition = Expression.Parameter(typeof(Rect));
			var paramProperty = Expression.Parameter(typeof(SerializedProperty));
			var paramLabel = Expression.Parameter(typeof(GUIContent));
			var paramIncludeChildren = Expression.Parameter(typeof(bool));

			var method =
				_PropertyHandlerType
					.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
					.First(m => m.Name == "OnGUI" && m.GetParameters().Length == 4);

			_PropertyHandler_OnGUI =
				Expression.Lambda<Func<object, Rect, SerializedProperty, GUIContent, bool, bool>>(
					Expression.Call(
						 Expression.TypeAs(paramHandler, _PropertyHandlerType), method,
						paramPosition,
						paramProperty,
						paramLabel,
						paramIncludeChildren),
					new[] { paramHandler, paramPosition, paramProperty, paramLabel, paramIncludeChildren })
					.Compile();
		}
	}
}
