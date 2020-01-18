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
		private IEnumerable<MethodInfo> _methods;

		private void OnEnable()
		{
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

			// Draw methods
			if (_methods.Any())
			{
				EditorGUILayout.LabelField("Buttons", EditorStyles.boldLabel);
				NaughtyEditorGUI.HorizontalLine(
					EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());

				foreach (var method in _methods)
				{
					NaughtyEditorGUI.Button(serializedObject.targetObject, method);
				}
			}
		}
	}
}
