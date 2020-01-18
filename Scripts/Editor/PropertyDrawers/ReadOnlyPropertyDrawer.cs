using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
	public class ReadOnlyPropertyDrawer : PropertyDrawerBase
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return GetPropertyHeight(property);
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			GUI.enabled = false;
			EditorGUI.PropertyField(rect, property, label, true);
			GUI.enabled = true;

			EditorGUI.EndProperty();
		}
	}
}