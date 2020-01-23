using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public class AssetsOnlyPropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            AssetsOnlyAttribute maxValueAttribute = PropertyUtility.GetAttribute<AssetsOnlyAttribute>(property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    if (!IsValid(property))
                    {
                        property.objectReferenceValue = null;
                    }
                }
            }
            else
            {
                string warning = maxValueAttribute.GetType().Name + " can be used only on object reference fields";
                Debug.LogWarning(warning, property.serializedObject.targetObject);
            }
        }
        protected virtual bool IsValid(SerializedProperty property)
        {
            if (AssetDatabase.IsMainAsset(property.objectReferenceValue))
            {
                return true;
            }
            return false;
        }
    }
}
