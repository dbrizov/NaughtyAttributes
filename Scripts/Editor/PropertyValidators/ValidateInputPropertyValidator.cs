using UnityEditor;
using System.Reflection;
using System;

namespace NaughtyAttributes.Editor
{
    public class ValidateInputPropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            ValidateInputAttribute validateInputAttribute = PropertyUtility.GetAttribute<ValidateInputAttribute>(property);
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            MethodInfo validationCallback = ReflectionUtility.GetMethod(target, validateInputAttribute.CallbackName);

            if (validationCallback != null &&
                validationCallback.ReturnType == typeof(bool))
            {
                ParameterInfo[] callbackParameters = validationCallback.GetParameters();

                if (callbackParameters.Length == 0)
                {
                    if (!(bool)validationCallback.Invoke(target, null))
                    {
                        if (string.IsNullOrEmpty(validateInputAttribute.Message))
                        {
                            NaughtyEditorGUI.HelpBox_Layout(
                                property.name + " is not valid", MessageType.Error, context: property.serializedObject.targetObject);
                        }
                        else
                        {
                            NaughtyEditorGUI.HelpBox_Layout(
                                validateInputAttribute.Message, MessageType.Error, context: property.serializedObject.targetObject);
                        }
                    }
                }
                else if (callbackParameters.Length == 1)
                {
                    FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
                    Type fieldType = fieldInfo.FieldType;
                    Type parameterType = callbackParameters[0].ParameterType;

                    if (fieldType == parameterType)
                    {
                        if (!(bool)validationCallback.Invoke(target, new object[] { fieldInfo.GetValue(target) }))
                        {
                            if (string.IsNullOrEmpty(validateInputAttribute.Message))
                            {
                                NaughtyEditorGUI.HelpBox_Layout(
                                    property.name + " is not valid", MessageType.Error, context: property.serializedObject.targetObject);
                            }
                            else
                            {
                                NaughtyEditorGUI.HelpBox_Layout(
                                    validateInputAttribute.Message, MessageType.Error, context: property.serializedObject.targetObject);
                            }
                        }
                    }
                    else
                    {
                        string warning = "The field type is not the same as the callback's parameter type";
                        NaughtyEditorGUI.HelpBox_Layout(warning, MessageType.Warning, context: property.serializedObject.targetObject);
                    }
                }
                else
                {
                    string warning =
                        validateInputAttribute.GetType().Name +
                        " needs a callback with boolean return type and an optional single parameter of the same type as the field";

                    NaughtyEditorGUI.HelpBox_Layout(warning, MessageType.Warning, context: property.serializedObject.targetObject);
                }
            }
        }
    }
}
