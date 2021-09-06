using System.Collections.Generic;
using System;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public class NaughtyProperty
	{
		public SerializedProperty property;
		public SpecialCaseDrawerAttribute specialCaseDrawerAttribute;
		public ShowIfAttributeBase showIfAttribute;

		public EnableIfAttributeBase enableIfAttribute;
		
		public ReadOnlyAttribute readOnlyAttribute;

		public ValidatorAttribute[] validatorAttributes;
	}
	
	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnityEngine.Object), true)]
	public class NaughtyInspector : UnityEditor.Editor
	{
		private List<NaughtyProperty> _serializedProperties = new List<NaughtyProperty>();
		private IEnumerable<FieldInfo> _nonSerializedFields;
		private IEnumerable<PropertyInfo> _nativeProperties;
		private IEnumerable<MethodInfo> _methods;

		private IEnumerable<NaughtyProperty> _nonGroupedSerializedProperty;

		private SerializedProperty m_ScriptProperty;

		private IEnumerable<IGrouping<string, NaughtyProperty>> _groupedSerialzedProperty;

		private IEnumerable<IGrouping<string, NaughtyProperty>> _foldoutGroupedSerializedProperty;
		
		private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();

		private bool _anyNaughtyAttribute;
		
		protected virtual void OnEnable()
		{
			_nonSerializedFields = ReflectionUtility.GetAllFields(
				target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

			_nativeProperties = ReflectionUtility.GetAllProperties(
				target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

			_methods = ReflectionUtility.GetAllMethods(
				target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

			GetSerializedProperties(ref _serializedProperties);
			
			_anyNaughtyAttribute = _serializedProperties.Any(p => PropertyUtility.GetAttribute<INaughtyAttribute>(p.property) != null);

			_nonGroupedSerializedProperty = GetNonGroupedProperties(_serializedProperties);
			NaughtyProperty[] mScripts = _serializedProperties.Where(p => p.property.name.Equals("m_Script")).ToArray();
			
			m_ScriptProperty = mScripts.Length > 0 ? mScripts[0].property : null;
			
			_groupedSerialzedProperty = GetGroupedProperties(_serializedProperties);

			_foldoutGroupedSerializedProperty = GetFoldoutProperties(_serializedProperties);
		}

		protected virtual void OnDisable()
		{
			ReorderableListPropertyDrawer.Instance.ClearCache();
		}
		

		public override void OnInspectorGUI()
		{
			if (!_anyNaughtyAttribute)
			{
				DrawDefaultInspector();
			}
			else
			{
				DrawSerializedProperties();
			}

			DrawNonSerializedFields();
			DrawNativeProperties();
			DrawButtons();
		}
		
		protected void GetSerializedProperties(ref List<NaughtyProperty> outSerializedProperties)
		{
			outSerializedProperties.Clear();
			using (var iterator = serializedObject.GetIterator())
			{
				if (iterator.NextVisible(true))
				{
					do
					{
						outSerializedProperties.Add(
							PropertyUtility.CreateNaughtyProperty(
								serializedObject.FindProperty(iterator.name)));
					}
					while (iterator.NextVisible(false));
				}
			}
		}

		protected void DrawSerializedProperties()
		{
			serializedObject.Update();

			if (m_ScriptProperty != null)
			{
				using (new EditorGUI.DisabledScope(disabled: true))
				{
					EditorGUILayout.PropertyField(m_ScriptProperty);
				}
			}

			// Draw non-grouped serialized properties
			foreach (var property in _nonGroupedSerializedProperty)
			{
				NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
			}

			// Draw grouped serialized properties
			foreach (var group in _groupedSerialzedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p.property));
				if (!visibleProperties.Any())
				{
					continue;
				}

				NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
				foreach (var property in visibleProperties)
				{
					NaughtyEditorGUI.PropertyField_Layout(property, includeChildren: true);
				}

				NaughtyEditorGUI.EndBoxGroup_Layout();
			}

			// Draw foldout serialized properties
			foreach (var group in _foldoutGroupedSerializedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p.property));
				if (!visibleProperties.Any())
				{
					continue;
				}

				if (!_foldouts.ContainsKey(group.Key))
				{
					_foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
				}

				_foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
				if (_foldouts[group.Key].Value)
				{
					foreach (var property in visibleProperties)
					{
						NaughtyEditorGUI.PropertyField_Layout(property, true);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		protected void DrawNonSerializedFields(bool drawHeader = false)
		{
			if (_nonSerializedFields.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var field in _nonSerializedFields)
				{
					NaughtyEditorGUI.NonSerializedField_Layout(serializedObject.targetObject, field);
				}
			}
		}

		protected void DrawNativeProperties(bool drawHeader = false)
		{
			if (_nativeProperties.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var property in _nativeProperties)
				{
					NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
				}
			}
		}

		protected void DrawButtons(bool drawHeader = false)
		{
			if (_methods.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var method in _methods)
				{
					NaughtyEditorGUI.Button(serializedObject.targetObject, method);
				}
			}
		}

		private static IEnumerable<NaughtyProperty> GetNonGroupedProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p.property) == null && !p.property.name.Equals("m_Script"));
		}

		private static IEnumerable<IGrouping<string, NaughtyProperty>> GetGroupedProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p.property) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p.property).Name);
		}

		private static IEnumerable<IGrouping<string, NaughtyProperty>> GetFoldoutProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p.property) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p.property).Name);
		}

		private static GUIStyle GetHeaderGUIStyle()
		{
			GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.UpperCenter;

			return style;
		}
	}
}
