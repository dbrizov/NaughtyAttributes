using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(InlineButtonAttribute))]
    public class InlineButtonPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var att = PropertyUtility.GetAttribute<InlineButtonAttribute>(property);
            var parent = GetParentObject(property.propertyPath, property.serializedObject.targetObject);
            var type = parent.GetType();
            var methodInfo = type.GetMethod(att.MethodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            var methodName = att.Label == null ? property.displayName : att.Label;

            EditorGUI.BeginChangeCheck();
            var guiContent = new GUIContent(property.displayName);
            EditorGUILayout.BeginHorizontal();
            {
                EditorGUILayout.PropertyField(property, guiContent, true);
                if (GUILayout.Button(methodName, GUILayout.ExpandWidth(att.ExpandButton)))
                {
                    methodInfo.Invoke(parent, null);
                }
            }
            EditorGUILayout.EndHorizontal();
        }

        // _NOTE: This one is also contained in the GetSetProperty
        private object GetParentObject(string path, object obj)
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
