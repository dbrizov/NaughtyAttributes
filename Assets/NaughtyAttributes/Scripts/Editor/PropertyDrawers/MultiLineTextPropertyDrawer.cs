using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(MultiLineTextAttribute))]
    public class MultiLineTextPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                EditorGUILayout.LabelField(property.displayName);

                EditorGUI.BeginChangeCheck();

                string textAreaValue = EditorGUILayout.TextArea(property.stringValue, GUILayout.MinHeight(EditorGUIUtility.singleLineHeight * 3f));

                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = textAreaValue;
                }
            }
            else
            {
                string warning = PropertyUtility.GetAttributes<MultiLineTextAttribute>(property)[0].GetType().Name + " can only be used on string fields";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning);

                EditorGUILayout.PropertyField(property, true);
            }
        }
    }
}
