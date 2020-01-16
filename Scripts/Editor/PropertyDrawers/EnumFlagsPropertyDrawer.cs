using UnityEngine;
using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsPropertyDrawer : PropertyDrawerBase
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			Enum targetEnum = PropertyUtility.GetTargetObjectOfProperty(property) as Enum;

			return (targetEnum != null)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			Enum targetEnum = PropertyUtility.GetTargetObjectOfProperty(property) as Enum;
			if (targetEnum != null)
			{
				Enum enumNew = EditorGUI.EnumFlagsField(position, property.displayName, targetEnum);
				property.intValue = (int)Convert.ChangeType(enumNew, targetEnum.GetType());
			}
			else
			{
				string message = attribute.GetType().Name + " can be used only on enums";
				DrawDefaultPropertyAndHelpBox(position, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}
	}
}
