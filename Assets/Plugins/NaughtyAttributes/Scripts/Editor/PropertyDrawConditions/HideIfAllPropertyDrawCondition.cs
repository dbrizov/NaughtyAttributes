using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawCondition(typeof(HideIfAllAttribute))]
    public class HideIfAllPropertyDrawCondition : PropertyDrawCondition
    {
        public override bool CanDrawProperty(SerializedProperty property)
        {
            HideIfAllAttribute[] hideIfAllAttributes = PropertyUtility.GetAttributes<HideIfAllAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            foreach (var attribute in hideIfAllAttributes)
            {
                FieldInfo conditionField = ReflectionUtility.GetField(target, attribute.ConditionName);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    if ((bool)conditionField.GetValue(target) != attribute.ConditionValue)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, attribute.ConditionName);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    if ((bool)conditionMethod.Invoke(target, null) != attribute.ConditionValue)
                    {
                        return true;
                    }
                    else
                    {
                        continue;
                    }
                }

                string warning = attribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }

            return false;
        }
    }
}
