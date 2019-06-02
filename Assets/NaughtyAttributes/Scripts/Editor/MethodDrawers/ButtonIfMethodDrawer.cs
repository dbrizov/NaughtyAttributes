using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonIfAttribute))]
    public class ButtonIfMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            ButtonIfAttribute buttonIfAttribute = (ButtonIfAttribute)methodInfo.GetCustomAttributes(typeof(ButtonIfAttribute), true)[0];

            List<bool> conditionValues = new List<bool>();
            foreach (var condition in buttonIfAttribute.Conditions)
            {
                //Debug.Log("condition: " + condition);

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
                    //Debug.Log("method found: " + condition);
                    conditionValues.Add((bool)conditionMethod.Invoke(target, null));
                }
            }

            bool enabled = false;
            if (conditionValues.Count > 0)
            {
                if (buttonIfAttribute.ConditionOperator == ConditionOperator.And)
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

                if (buttonIfAttribute.Reversed)
                {
                    enabled = !enabled;
                }
            }
            else
            {
                string warning = buttonIfAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
                return;
            }

            if (methodInfo.GetParameters().Length == 0)
            {
                string buttonText = string.IsNullOrEmpty(buttonIfAttribute.Text) ? methodInfo.Name : buttonIfAttribute.Text;

                GUI.enabled = enabled;
                if (GUILayout.Button(buttonText))
                {
                    methodInfo.Invoke(target, null);
                }
                GUI.enabled = true;
                if (!enabled)
                {
                    string infoText = "Preconditions for " + buttonText + ":";
                    for (int ix = 0; ix < conditionValues.Count; ix++)
                    {
                        infoText += "\n  ";
                        if (ix > 0)
                            infoText +=
                                buttonIfAttribute.ConditionOperator + " ";
                        infoText += buttonIfAttribute.Conditions[ix];
                        infoText += " (" + conditionValues[ix] + ")";
                    }
                    EditorGUILayout.HelpBox(infoText, MessageType.Info);
                }
            }
            else
            {
                string warning = typeof(ButtonIfAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
