using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawCondition(typeof(ShowIfAttribute))]
    public class ShowIfPropertyDrawCondition : PropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            ShowIfAttribute showIfAttribute = PropertyUtility.GetAttributes<ShowIfAttribute>(property)[0];
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo conditionField = target.GetType().GetField(showIfAttribute.ConditionName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                return (bool)conditionField.GetValue(target);
            }

            MethodInfo conditionMethod = target.GetType().GetMethod(showIfAttribute.ConditionName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                return (bool)conditionMethod.Invoke(target, null);
            }

            string warning = showIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
            EditorGUILayout.HelpBox(warning, MessageType.Warning);
            Debug.LogWarning(warning, target);

            return true;
        }
    }
}
