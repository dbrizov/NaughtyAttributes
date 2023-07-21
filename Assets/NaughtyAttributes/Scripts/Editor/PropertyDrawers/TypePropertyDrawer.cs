using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(TypeAttribute))]
    public class TypeAttributePropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            if (attribute is TypeAttribute attr && property.type == "string")
            {
                GUIContent[] TypeNames = attr.types.Select(type => new GUIContent(type.FullName)).ToArray();
                int index = Array.FindIndex(attr.types, type => type.FullName == property.stringValue);
                index = EditorGUI.Popup(rect, label, index, TypeNames);
                if (index >= 0)
                {
                    property.stringValue = attr.types[index].FullName;
                }

            }else{
                string message = string.Format("{0} supports only string fields", typeof(TypeAttribute).Name);
                EditorGUI.LabelField(rect, label, new GUIContent(message));

            }
        }
    }
}
