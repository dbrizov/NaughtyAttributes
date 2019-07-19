using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(GetSetAttribute))]
    public class GetSetPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var getSetAttribute = PropertyUtility.GetAttribute<GetSetAttribute>(property);

            EditorGUI.BeginChangeCheck();
            var guiContent = new GUIContent(property.displayName);
            EditorGUILayout.PropertyField(property, guiContent, true);

            if (EditorGUI.EndChangeCheck())
            {
                var parent = GetParentObject(property.propertyPath, property.serializedObject.targetObject);
                var type = parent.GetType();
                var fieldInfo = type.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
                var propertyInfo = type.GetProperty(getSetAttribute.PropertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

                if (propertyInfo == null)
                {
                    Debug.LogError("Invalid property name \"" + getSetAttribute.PropertyName + "\"");
                }
                else
                {
                    propertyInfo.SetValue(parent, fieldInfo.GetValue(parent), null);
                }
            }
        }

        public static object GetParentObject(string path, object obj)
        {
            var fields = path.Split('.');

            if (fields.Length == 1)
            {
                return obj;
            }

            FieldInfo info = obj.GetType().GetField(fields[0], BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            obj = info.GetValue(obj);

            return GetParentObject(string.Join(".", fields, 1, fields.Length - 1), obj);
        }
    }
}
