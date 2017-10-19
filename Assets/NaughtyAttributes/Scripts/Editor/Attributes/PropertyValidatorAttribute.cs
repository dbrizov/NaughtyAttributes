using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyValidatorAttribute : BasePropertyAttribute
    {
        public PropertyValidatorAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
