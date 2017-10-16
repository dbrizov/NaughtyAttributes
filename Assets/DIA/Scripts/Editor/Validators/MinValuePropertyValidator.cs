using UnityEditor;

[PropertyValidator(typeof(MinValueAttribute))]
public class MinValuePropertyValidator : PropertyValidator
{
    protected override void ValidatePropertyImplementation(SerializedProperty property)
    {
        MinValueAttribute minValueAttribute = PropertyUtility.GetAttributes<MinValueAttribute>(property)[0];

        if (property.propertyType == SerializedPropertyType.Float)
        {
            if (property.floatValue < minValueAttribute.MinValue)
            {
                property.floatValue = minValueAttribute.MinValue;
            }
        }
        else
        {
            if (property.intValue < minValueAttribute.MinValue)
            {
                property.intValue = (int)minValueAttribute.MinValue;
            }
        }
    }
}