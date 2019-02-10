using System.Collections;
using System.Collections.Generic;
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
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            var guiContent = new GUIContent(labelAttribute.label);
            EditorGUILayout.PropertyField(property, guiContent, true);
        }

    }
}
