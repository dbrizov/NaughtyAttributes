using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(OnlySceneObjectsAttribute))]
    public class OnlySceneObjectsPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var onlySceneObjectsAttribute = PropertyUtility.GetAttribute<OnlySceneObjectsAttribute>(property);
            bool logToConsole = onlySceneObjectsAttribute.LogToConsole;

            var targetObject = property.serializedObject.targetObject;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            string error = property.displayName + " Must Be a Scene Object";

            if (field != null)
            {
                var value = field.GetValue(targetObject);
                if (value != null && value.ToString() != "null" && AssetDatabase.Contains(value as Object))
                {
                    EditorDrawUtility.DrawHelpBox(error, MessageType.Error, context: PropertyUtility.GetTargetObject(property), logToConsole: logToConsole);
                }
            }

            field.SetValue
            (
                targetObject,
                EditorGUILayout.ObjectField(property.displayName, field.GetValue(targetObject) as Object, field.FieldType, true)
            );
        }
    }
}
