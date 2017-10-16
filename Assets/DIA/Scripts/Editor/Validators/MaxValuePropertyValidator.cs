using UnityEditor;

[PropertyValidator(typeof(MaxValueAttribute))]
public class MaxValuePropertyValidator : PropertyValidator
{
    protected override void ValidatePropertyImplementation(SerializedProperty property)
    {
        MaxValueAttribute maxValueAttribute = PropertyUtility.GetAttributes<MaxValueAttribute>(property)[0];

        if (property.propertyType == SerializedPropertyType.Float)
        {
            if (property.floatValue > maxValueAttribute.MaxValue)
            {
                property.floatValue = maxValueAttribute.MaxValue;
            }
        }
        else
        {
            if (property.intValue > maxValueAttribute.MaxValue)
            {
                property.intValue = (int)maxValueAttribute.MaxValue;
            }
        }
    }
}