using System.Text;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    public class RequiredTypePropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            RequiredTypeAttribute requiredTypeAttribute = PropertyUtility.GetAttribute<RequiredTypeAttribute>(property);

            if (requiredTypeAttribute == null)
            {
                return;
            }

            if (property.propertyType != SerializedPropertyType.ObjectReference)
            {
                NaughtyEditorGUI.HelpBox_Layout(requiredTypeAttribute.GetType().Name + " works only on reference types",
                    MessageType.Warning,
                    context: property.serializedObject.targetObject
                );
                return;
            }

            if (property.objectReferenceValue == null)
            {
                if (requiredTypeAttribute.ShowInfoMessageWhenEmpty)
                {
                    StringBuilder infoMessage = new StringBuilder();
                    infoMessage.AppendLine(property.name + " must have ");
                    
                    foreach (var baseType in requiredTypeAttribute.BaseTypes)
                    {
                        infoMessage.AppendLine("\"" + baseType.FullName + "\"");
                    }

                    infoMessage.Append(requiredTypeAttribute.BaseTypes.Length > 1
                        ? " or one of their derived types"
                        : " or derived type");

                    NaughtyEditorGUI.HelpBox_Layout(infoMessage.ToString(), MessageType.Info,
                        context: property.serializedObject.targetObject);
                }

                return;
            }

            if (HasGameObject(property.objectReferenceValue, out GameObject gameObject))
            {
                bool hasValidationError = false;
                StringBuilder errorMessage = new StringBuilder();

                foreach (var baseType in requiredTypeAttribute.BaseTypes)
                {
                    var hasComponent = gameObject.GetComponent(baseType);
                    if (!hasComponent)
                    {
                        hasValidationError = true;
                        errorMessage.AppendLine(property.name + " must have \"" + baseType.FullName + "\" or derived type");
                    }
                }

                if (hasValidationError)
                {
                    NaughtyEditorGUI.HelpBox_Layout(errorMessage.ToString(), MessageType.Error,
                        context: property.serializedObject.targetObject);
                }
            }
            else
            {
                NaughtyEditorGUI.HelpBox_Layout(
                    requiredTypeAttribute.GetType().Name + " works only on types has GameObject",
                    MessageType.Warning,
                    context: property.serializedObject.targetObject
                );
            }
        }

        private bool HasGameObject(Object obj, out GameObject gameObject)
        {
            if (obj is GameObject go)
            {
                gameObject = go;
                return true;
            }

            if (obj is Component component)
            {
                gameObject = component.gameObject;
                return gameObject != null;
            }

            gameObject = null;
            return false;
        }
    }
}