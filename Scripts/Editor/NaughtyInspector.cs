using System.Collections.Generic;
using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnityEngine.Object), true)]
	public class NaughtyInspector : UnityEditor.Editor
	{
		private IEnumerable<FieldInfo> _serializedFields;

		private void OnEnable()
		{
			_serializedFields = ReflectionUtility.GetAllFields(target, f => serializedObject.FindProperty(f.Name) != null);
		}

		private void OnDisable()
		{
			SpecialCasePropertyDrawerDatabase.ClearCache();
		}

		public override void OnInspectorGUI()
		{
			serializedObject.Update();

			foreach (var field in _serializedFields)
			{
				SerializedProperty property = serializedObject.FindProperty(field.Name);
				EditorGUIExtensions._PropertyField_Layout(property, true);
			}

			serializedObject.ApplyModifiedProperties();
		}
	}
}
