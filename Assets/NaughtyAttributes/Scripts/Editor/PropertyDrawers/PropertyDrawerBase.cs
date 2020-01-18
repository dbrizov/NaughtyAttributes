using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public abstract class PropertyDrawerBase : PropertyDrawer
	{
		public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			bool enabled = PropertyUtility.IsEnabled(property);
			GUI.enabled = enabled;
			OnGUI_Internal(rect, property, label);
			GUI.enabled = true;
		}

		protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);

		public virtual float GetPropertyHeight(SerializedProperty property)
		{
			return EditorGUI.GetPropertyHeight(property, true);
		}

		public virtual float GetHelpBoxHeight()
		{
			return EditorGUIUtility.singleLineHeight * 3.0f;
		}

		public void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message, MessageType messageType)
		{
			Rect helpBoxRect = new Rect(
					rect.x + NaughtyEditorGUI.GetIndentLength(rect),
					rect.y,
					rect.width,
					GetHelpBoxHeight() - 2.0f);

			NaughtyEditorGUI.HelpBox(helpBoxRect, message, MessageType.Warning, property.serializedObject.targetObject);

			Rect propertyRect = new Rect(
				rect.x,
				rect.y + GetHelpBoxHeight(),
				rect.width,
				GetPropertyHeight(property));

			EditorGUI.PropertyField(propertyRect, property, true);
		}
	}
}
