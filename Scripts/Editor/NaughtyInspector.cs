using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnityEngine.Object), true)]
	public class NaughtyInspector : UnityEditor.Editor
	{
		private IEnumerable<FieldInfo> _nonSerializedFields;
		private IEnumerable<PropertyInfo> _nativeProperties;
		private IEnumerable<MethodInfo> _methods;

		private void OnEnable()
		{
			_nonSerializedFields = ReflectionUtility.GetAllFields(
				target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

			_nativeProperties = ReflectionUtility.GetAllProperties(
				target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

			_methods = ReflectionUtility.GetAllMethods(
				target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);
		}

		private void OnDisable()
		{
			ReorderableListPropertyDrawer.Instance.ClearCache();
		}

		public override void OnInspectorGUI()
		{
			// Draw serialized properties
			serializedObject.Update();

			using (var iterator = serializedObject.GetIterator())
			{
				if (iterator.NextVisible(true))
				{
					do
					{
						if (iterator.name.Equals("m_Script", System.StringComparison.Ordinal))
						{
							GUI.enabled = false;
							SerializedProperty property = serializedObject.FindProperty(iterator.name);
							EditorGUILayout.PropertyField(property);
							GUI.enabled = true;
						}
						else
						{
							SerializedProperty property = serializedObject.FindProperty(iterator.name);
							NaughtyEditorGUI.PropertyField_Layout(property, true);
						}
					}
					while (iterator.NextVisible(false));
				}
			}

			serializedObject.ApplyModifiedProperties();

			// Draw non-serialized fields
			if (_nonSerializedFields.Any())
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
				NaughtyEditorGUI.HorizontalLine(
					EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());

				foreach (var field in _nonSerializedFields)
				{
					NaughtyEditorGUI.NonSerializedField_Layout(serializedObject.targetObject, field);
				}
			}

			// Draw native properties
			if (_nativeProperties.Any())
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
				NaughtyEditorGUI.HorizontalLine(
					EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());

				foreach (var property in _nativeProperties)
				{
					NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
				}
			}

			// Draw methods
			if (_methods.Any())
			{
				EditorGUILayout.Space();
				EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
				NaughtyEditorGUI.HorizontalLine(
					EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());

				foreach (var method in _methods)
				{
					NaughtyEditorGUI.Button(serializedObject.targetObject, method);
				}
			}
		}

		private GUIStyle GetHeaderGUIStyle()
		{
			GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.UpperCenter;

			return style;
		}
	}
}
