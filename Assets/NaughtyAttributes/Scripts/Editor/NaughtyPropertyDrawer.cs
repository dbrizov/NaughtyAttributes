using System;
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
			if (!PropertyUtility.IsVisible(property))
				return 0;

			var handler = _ScriptAttributeUtility_GetPropertyHandler(property);

			return _PropertyHandler_GetHeight(handler, property, label);
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{		
			#if UNITY_2021 || UNITY_2020
			var includeChildren = _EditorGUI_HasVisibleChildFields(property, false);
			#else
			var includeChildren = _EditorGUI_HasVisibleChildFields(property);
			#endif

			NaughtyEditorGUI.PropertyField_Implementation(position, property, includeChildren, DefaultOnGUI);
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
		
		// This method returns whether a serialized object has any visible child fields
		// Unity passes result of this method down the line as value for the includeChildren parameters
		#if UNITY_2021 || UNITY_2020
		private static Func<SerializedProperty, bool, bool> _EditorGUI_HasVisibleChildFields;
		#else
		private static Func<SerializedProperty, bool> _EditorGUI_HasVisibleChildFields;
		#endif

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


			#if UNITY_2021 || UNITY_2020
			_EditorGUI_HasVisibleChildFields = (Func<SerializedProperty, bool, bool>)
				typeof(EditorGUI)
					.GetMethod("HasVisibleChildFields", kAnyStaticMemberBindingFlags)
					.CreateDelegate(typeof(Func<SerializedProperty, bool, bool>), null);
			#else
			_EditorGUI_HasVisibleChildFields = (Func<SerializedProperty, bool>)
				typeof(EditorGUI)
					.GetMethod("HasVisibleChildFields", kAnyStaticMemberBindingFlags)
					.CreateDelegate(typeof(Func<SerializedProperty, bool>), null);
			#endif


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
			var PropertyHandler_m_PropertyDrawerField = _PropertyHandlerType.GetField("m_PropertyDrawer", kAnyInstanceMemberBindingFlags);

			var sharedNullHandler = ScriptAttributeUtility_sharedNullHandlerField.GetValue(null);

			PropertyHandler_m_PropertyDrawerField.SetValue(sharedNullHandler, new NaughtyPropertyDrawer());
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
						#if UNITY_2021 || UNITY_2020
						Expression.Call(null, _EditorGUI_HasVisibleChildFields.Method, paramProperty, Expression.Constant(false))),
						#else
						Expression.Call(null, _EditorGUI_HasVisibleChildFields.Method, paramProperty)),
						#endif
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
