using System;
using System.Reflection;
using UnityEditor;

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
                            EditorDrawUtility.DrawHelpBox(property.name + " is not valid", MessageType.Error, context: target);
                        }
                        else
                        {
                            EditorDrawUtility.DrawHelpBox(validateInputAttribute.Message, MessageType.Error, context: target);
                        }
                    }
                }
                else
                {
                    string warning = "The field type is not the same as the callback's parameter type";
                    EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
                }
            }
            else
            {
                string warning =
                    validateInputAttribute.GetType().Name +
                    " needs a callback with boolean return type and a single parameter of the same type as the field";

                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
