using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(LabelAttribute))]
    public class LabelPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var labelAttribute = PropertyUtility.GetAttribute<LabelAttribute>(property);
            var guiContent = new GUIContent(labelAttribute.Label);
            EditorGUILayout.PropertyField(property, guiContent, true);
        }
    }
}
