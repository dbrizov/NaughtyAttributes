using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(DisableIfAttribute))]
    public class DisableIfPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            bool drawDisabled = false;
            bool validCondition = false;

            DisableIfAttribute disableIfAttribute = PropertyUtility.GetAttribute<DisableIfAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo conditionField = ReflectionUtility.GetField(target, disableIfAttribute.ConditionName);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
                drawDisabled = (bool)conditionField.GetValue(target);
                validCondition = true;
            }

            MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, disableIfAttribute.ConditionName);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                drawDisabled = (bool)conditionMethod.Invoke(target, null);
                validCondition = true;
            }

            if (validCondition)
            {
                GUI.enabled = !drawDisabled;
                EditorDrawUtility.DrawPropertyField(property);
                GUI.enabled = true;
            }
            else
            {
                string warning = disableIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, logToConsole: true, context: target);
            }
        }
    }
}
