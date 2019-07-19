using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(OnlyAssetsAttribute))]
    public class OnlyAssetsPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            var onlyAssetsAttribute = PropertyUtility.GetAttribute<OnlyAssetsAttribute>(property);
            bool logToConsole = onlyAssetsAttribute.LogToConsole;

            var targetObject = property.serializedObject.targetObject as object;
            var targetObjectClassType = targetObject.GetType();
            var field = targetObjectClassType.GetField(property.propertyPath, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            string error = property.displayName + " Must Be an Asset";

            if (field != null)
            {
                var value = field.GetValue(targetObject);
                if (value != null && value.ToString() != "null" && AssetDatabase.Contains(value as Object) == false)
                {
                    EditorDrawUtility.DrawHelpBox(error, MessageType.Error, context: PropertyUtility.GetTargetObject(property), logToConsole: logToConsole);
                }
            }

            field.SetValue
            (
                targetObject,
                EditorGUILayout.ObjectField(property.displayName, field.GetValue(targetObject) as Object, field.FieldType, false)
            );
        }
    }
}
