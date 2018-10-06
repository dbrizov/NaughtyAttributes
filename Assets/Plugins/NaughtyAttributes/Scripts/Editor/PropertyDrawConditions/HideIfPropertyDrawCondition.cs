using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawCondition(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawCondition : PropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            HideIfAttribute hideIfAttribute = PropertyUtility.GetAttribute<HideIfAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo conditionField = ReflectionUtility.GetField(target, hideIfAttribute.ConditionName);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                return !(bool)conditionField.GetValue(target);
            }

            MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, hideIfAttribute.ConditionName);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                return !(bool)conditionMethod.Invoke(target, null);
            }

            string warning = hideIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
            EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);

            return true;
        }
    }
}
