using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	public class MinMaxSliderPropertyDrawer : PropertyDrawerBase
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return (property.propertyType == SerializedPropertyType.Vector2)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			MinMaxSliderAttribute minMaxSliderAttribute = (MinMaxSliderAttribute)attribute;

			if (property.propertyType == SerializedPropertyType.Vector2)
			{
				EditorGUI.BeginProperty(position, label, property);

				float indentLength = NaughtyEditorGUI.GetIndentLength(position);
				float labelWidth = EditorGUIUtility.labelWidth;
				float floatFieldWidth = EditorGUIUtility.fieldWidth;
				float sliderWidth = position.width - labelWidth - 2f * floatFieldWidth;
				float sliderPadding = 5f;

				Rect labelRect = new Rect(
					position.x,
					position.y,
					labelWidth,
					position.height);

				Rect sliderRect = new Rect(
					position.x + labelWidth + floatFieldWidth + sliderPadding - indentLength,
					position.y,
					sliderWidth - 2f * sliderPadding + indentLength,
					position.height);

				Rect minFloatFieldRect = new Rect(
					position.x + labelWidth - indentLength,
					position.y,
					floatFieldWidth + indentLength,
					position.height);

				Rect maxFloatFieldRect = new Rect(
					position.x + labelWidth + floatFieldWidth + sliderWidth - indentLength,
					position.y,
					floatFieldWidth + indentLength,
					position.height);

				// Draw the label
				EditorGUI.LabelField(labelRect, property.displayName);

				// Draw the slider
				EditorGUI.BeginChangeCheck();

				Vector2 sliderValue = property.vector2Value;
				EditorGUI.MinMaxSlider(sliderRect, ref sliderValue.x, ref sliderValue.y, minMaxSliderAttribute.MinValue, minMaxSliderAttribute.MaxValue);

				sliderValue.x = EditorGUI.FloatField(minFloatFieldRect, sliderValue.x);
				sliderValue.x = Mathf.Clamp(sliderValue.x, minMaxSliderAttribute.MinValue, Mathf.Min(minMaxSliderAttribute.MaxValue, sliderValue.y));

				sliderValue.y = EditorGUI.FloatField(maxFloatFieldRect, sliderValue.y);
				sliderValue.y = Mathf.Clamp(sliderValue.y, Mathf.Max(minMaxSliderAttribute.MinValue, sliderValue.x), minMaxSliderAttribute.MaxValue);

				if (EditorGUI.EndChangeCheck())
				{
					property.vector2Value = sliderValue;
				}

				EditorGUI.EndProperty();
			}
			else
			{
				string message = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
				DrawDefaultPropertyAndHelpBox(position, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}
	}
}
