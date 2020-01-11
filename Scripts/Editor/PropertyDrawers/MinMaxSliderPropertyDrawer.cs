using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
	public class MinMaxSliderPropertyDrawer : NaughtyPropertyDrawer
	{
		private static readonly float MinHeight = EditorGUIUtility.singleLineHeight;
		private static readonly float MaxHeight = EditorGUIUtility.singleLineHeight * 4.0f;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			if (property.propertyType == SerializedPropertyType.Vector2)
			{
				return MinHeight;
			}
			else
			{
				return MaxHeight;
			}
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			MinMaxSliderAttribute minMaxSliderAttribute = (MinMaxSliderAttribute)attribute;

			if (property.propertyType == SerializedPropertyType.Vector2)
			{
				EditorGUI.BeginProperty(position, label, property);

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
					position.x + labelWidth + floatFieldWidth + sliderPadding,
					position.y,
					sliderWidth - 2f * sliderPadding,
					position.height);

				Rect minFloatFieldRect = new Rect(
					position.x + labelWidth,
					position.y,
					floatFieldWidth,
					position.height);

				Rect maxFloatFieldRect = new Rect(
					position.x + labelWidth + floatFieldWidth + sliderWidth,
					position.y,
					floatFieldWidth,
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
				Rect warningRect = new Rect(
					position.x,
					position.y,
					position.width,
					MaxHeight * 0.7f);

				string warning = minMaxSliderAttribute.GetType().Name + " can be used only on Vector2 fields";
				EditorDrawUtility.DrawHelpBox(warningRect, warning, MessageType.Warning, context: GetTargetObject(property));

				Rect propertyRect = new Rect(
					position.x,
					position.y + MaxHeight * 0.75f,
					position.width,
					MaxHeight * 0.25f);

				EditorDrawUtility.DrawPropertyField(propertyRect, property);
			}

			EditorGUI.EndProperty();
		}
	}
}
