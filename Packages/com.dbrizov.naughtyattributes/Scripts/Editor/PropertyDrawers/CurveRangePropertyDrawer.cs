using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(CurveRangeAttribute))]
	public class CurveRangePropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			float propertyHeight = property.propertyType == SerializedPropertyType.AnimationCurve
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			// Check user error
			if (property.propertyType != SerializedPropertyType.AnimationCurve)
			{
				string message = string.Format("Field {0} is not an AnimationCurve", property.name);
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				return;
			}

			var attribute = PropertyUtility.GetAttribute<CurveRangeAttribute>(property);

			EditorGUI.CurveField(rect, property,
				attribute.Color == EColor.Clear ? Color.green : attribute.Color.GetColor(),
				new Rect(attribute.Min.x, attribute.Min.y, attribute.Max.x - attribute.Min.x, attribute.Max.y - attribute.Min.y));

			EditorGUI.EndProperty();
		}
	}
}
