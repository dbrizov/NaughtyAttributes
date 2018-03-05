using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyValidator(typeof(ValidateInputAttribute))]
    public class ValidateInputPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            MethodInfo validationCallback = ReflectionUtility.GetMethod(target, validateInputAttribute.CallbackName);

            if (validationCallback != null &&
                validationCallback.ReturnType == typeof(bool) &&
                validationCallback.GetParameters().Length == 1)
            {
                FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
                Type fieldType = fieldInfo.FieldType;
                Type parameterType = validationCallback.GetParameters()[0].ParameterType;

                if (fieldType == parameterType)
                {
                    if (!(bool)validationCallback.Invoke(target, new object[] { fieldInfo.GetValue(target) }))
                    {
                        if (string.IsNullOrEmpty(validateInputAttribute.Message))
                        {
                            this.DrawHelpBox(property.name + " is not valid", target, MessageType.Error);
                        }
                        else
                        {
                            this.DrawHelpBox(validateInputAttribute.Message, target, MessageType.Error);
                        }
                    }
                }
                else
                {
                    this.DrawHelpBox("The field type is not the same as the callback's parameter type", target, MessageType.Warning);
                }
            }
            else
            {
                this.DrawHelpBox(validateInputAttribute.GetType().Name +
                    " needs a callback with boolean return type and a single parameter of the same type as the field", target, MessageType.Warning);
            }
        }

        private void DrawHelpBox(string message, UnityEngine.Object target, MessageType messageType)
        {
            EditorGUILayout.HelpBox(message, messageType);

            switch (messageType)
            {
                case MessageType.Warning:
                    Debug.LogWarning(message, target);
                    break;

                case MessageType.Error:
                    Debug.LogError(message, target);
                    break;

                default:
                    Debug.Log(message, target);
                    break;
            }
        }
    }
}
