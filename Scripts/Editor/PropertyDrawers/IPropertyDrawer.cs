using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public interface IPropertyDrawer
	{
		float GetPropertyHeight(SerializedProperty property);
		float GetHelpBoxHeight();
		void DrawDefaultPropertyAndHelpBox(Rect rect, SerializedProperty property, string message, MessageType messageType);
	}

	public class PropertyDrawerBase : PropertyDrawer, IPropertyDrawer
	{
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
