using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(CurveRangeAttribute))]
    public class CurveRangeAttributePropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            var attribute = PropertyUtility.GetAttribute<CurveRangeAttribute>(property);
            EditorGUI.BeginProperty(rect, label, property);
            //EditorGUI.PropertyField(rect, property, label, true);
            EditorGUI.CurveField(rect, property, attribute.color.GetColor(),
                new Rect(attribute.x, attribute.y, attribute.width, attribute.height));
            EditorGUI.EndProperty();
        }
    }
}
