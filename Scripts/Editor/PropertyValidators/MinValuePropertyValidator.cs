using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public class MinValuePropertyValidator : PropertyValidatorBase
    {
        public override void ValidateProperty(SerializedProperty property)
        {
            MinValueAttribute minValueAttribute = PropertyUtility.GetAttribute<MinValueAttribute>(property);

            if (property.propertyType == SerializedPropertyType.Integer)
            {
                if (property.intValue < minValueAttribute.MinValue)
                {
                    property.intValue = (int)minValueAttribute.MinValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Float)
            {
                if (property.floatValue < minValueAttribute.MinValue)
                {
                    property.floatValue = minValueAttribute.MinValue;
                }
            }
            else if (property.propertyType == SerializedPropertyType.Vector2)
            {
                property.vector2Value = Vector2.Max(property.vector2Value, new Vector2(minValueAttribute.MinValue, minValueAttribute.MinValue));
            }
            else if (property.propertyType == SerializedPropertyType.Vector3)
            {
                property.vector3Value = Vector3.Max(property.vector3Value, new Vector3(minValueAttribute.MinValue, minValueAttribute.MinValue, minValueAttribute.MinValue));
            }
            else if (property.propertyType == SerializedPropertyType.Vector4)
            {
                property.vector4Value = Vector4.Max(property.vector4Value, new Vector4(minValueAttribute.MinValue, minValueAttribute.MinValue, minValueAttribute.MinValue, minValueAttribute.MinValue));
            }
            else if (property.propertyType == SerializedPropertyType.Vector2Int)
            {
                property.vector2IntValue = Vector2Int.Max(property.vector2IntValue, new Vector2Int((int)minValueAttribute.MinValue, (int)minValueAttribute.MinValue));
            }
            else if (property.propertyType == SerializedPropertyType.Vector3Int)
            {
                property.vector3IntValue = Vector3Int.Max(property.vector3IntValue, new Vector3Int((int)minValueAttribute.MinValue, (int)minValueAttribute.MinValue, (int)minValueAttribute.MinValue));
            }
            else
            {
                string warning = minValueAttribute.GetType().Name + " can be used only on int, float, Vector or VectorInt fields";
                Debug.LogWarning(warning, property.serializedObject.targetObject);
            }
        }
    }
}
