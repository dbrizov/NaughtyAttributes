using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(CurveRangeAttribute))]
    public class CurveRangeAttributePropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            // Check user error
            if (property.propertyType != SerializedPropertyType.AnimationCurve)
            {
                string message = string.Format("Field {0} is not an AnimationCurve", property.name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
                return;
            }

            var attribute = PropertyUtility.GetAttribute<CurveRangeAttribute>(property);
            EditorGUI.BeginProperty(rect, label, property);

            EditorGUI.CurveField(rect, property, 
                attribute.color == EColor.Clear ? Color.green : attribute.color.GetColor(),
                new Rect(attribute.x, attribute.y, attribute.width, attribute.height));

            EditorGUI.EndProperty();
        }
    }
}
