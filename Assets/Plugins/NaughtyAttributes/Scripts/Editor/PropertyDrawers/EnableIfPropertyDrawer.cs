using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EnableIfAttribute enableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            List<bool> conditionValues = new List<bool>();
            foreach (var condition in enableIfAttribute.Conditions)
            {
                FieldInfo conditionField = ReflectionUtility.GetField(target, condition);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    conditionValues.Add((bool)conditionField.GetValue(target));
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, condition);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
                }
            }

            if (conditionValues.Count > 0)
            {
                bool enabled;
                if (enableIfAttribute.ConditionOperator == ConditionOperator.And)
                {
                    enabled = true;
                    foreach (var value in conditionValues)
                    {
                        enabled = enabled && value;
                    }
                }
                else
                {
                    enabled = false;
                    foreach (var value in conditionValues)
                    {
                        enabled = enabled || value;
                    }
                }

                if (enableIfAttribute.Reversed)
                {
                    enabled = !enabled;
                }

                GUI.enabled = enabled;
                EditorDrawUtility.DrawPropertyField(property);
                GUI.enabled = true;
            }
            else
            {
                string warning = enableIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
