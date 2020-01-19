using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public abstract class PropertyDrawerBase : PropertyDrawer
	{
		public sealed override void OnGUI(Rect rect, SerializedProperty property, GUIContent label)
		{
			bool visible = PropertyUtility.IsVisible(property);
			if (!visible)
			{
				return;
			}

			bool enabled = PropertyUtility.IsEnabled(property);
			GUI.enabled = enabled;

			GUIContent overrideLabel = new GUIContent(PropertyUtility.GetLabel(property));
			OnGUI_Internal(rect, property, overrideLabel);

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
			float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
			Rect helpBoxRect = new Rect(
					rect.x + indentLength,
					rect.y,
					rect.width - indentLength,
					GetHelpBoxHeight() - 2.0f);

			NaughtyEditorGUI.HelpBox(helpBoxRect, message, MessageType.Warning, context: property.serializedObject.targetObject);

			Rect propertyRect = new Rect(
				rect.x,
				rect.y + GetHelpBoxHeight(),
				rect.width,
				GetPropertyHeight(property));

			EditorGUI.PropertyField(propertyRect, property, true);
		}
	}
}
