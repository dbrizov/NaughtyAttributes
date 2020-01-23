using UnityEditor;
using UnityEngine;
namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(SliderAttribute))]
    public class SliderPropertyDrawer : PropertyDrawerBase
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.Float)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            SliderAttribute sliderAttribute = (SliderAttribute)attribute;

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                EditorGUI.BeginProperty(rect, label, property);

                float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
                float labelWidth = EditorGUIUtility.labelWidth;
                float sliderWidth = rect.width - labelWidth;

                Rect labelRect = new Rect(
                    rect.x,
                    rect.y,
                    labelWidth,
                    rect.height);

                Rect sliderRect = new Rect(
                    rect.x + labelWidth - indentLength,
                    rect.y,
                    sliderWidth + indentLength,
                    rect.height);


                // Draw the label
                EditorGUI.LabelField(labelRect, label.text);

                // Draw the slider
                EditorGUI.BeginChangeCheck();

                int sliderValue = property.intValue;
                sliderValue = EditorGUI.IntSlider(sliderRect, sliderValue, (int)sliderAttribute.MinValue, (int)sliderAttribute.MaxValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.intValue = sliderValue;
                }

                EditorGUI.EndProperty();
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                EditorGUI.BeginProperty(rect, label, property);

                float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
                float labelWidth = EditorGUIUtility.labelWidth;
                float sliderWidth = rect.width - labelWidth;

                Rect labelRect = new Rect(
                    rect.x,
                    rect.y,
                    labelWidth,
                    rect.height);

                Rect sliderRect = new Rect(
                    rect.x + labelWidth - indentLength,
                    rect.y,
                    sliderWidth + indentLength,
                    rect.height);


                // Draw the label
                EditorGUI.LabelField(labelRect, label.text);

                // Draw the slider
                EditorGUI.BeginChangeCheck();

                float sliderValue = property.floatValue;
                sliderValue = EditorGUI.Slider(sliderRect, sliderValue, sliderAttribute.MinValue, sliderAttribute.MaxValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.floatValue = sliderValue;
                }

                EditorGUI.EndProperty();
            }
            else
            {
                string message = sliderAttribute.GetType().Name + " can be used only on int or float fields";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}
