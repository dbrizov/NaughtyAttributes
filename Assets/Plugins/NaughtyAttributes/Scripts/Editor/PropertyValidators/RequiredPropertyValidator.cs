using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyValidator(typeof(RequiredAttribute))]
    public class RequiredPropertyValidator : PropertyValidator
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            RequiredAttribute requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    string errorMessage = property.name + " is required";
                    if (!string.IsNullOrEmpty(requiredAttribute.Message))
                    {
                        errorMessage = requiredAttribute.Message;
                    }

                    EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
                    Debug.LogError(errorMessage, PropertyUtility.GetTargetObject(property));
                }
            }
            else
            {
                string warning = requiredAttribute.GetType().Name + " works only on reference types";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, PropertyUtility.GetTargetObject(property));
            }
        }
    }
}
