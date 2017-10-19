using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyDrawConditionAttribute : BasePropertyAttribute
    {
        public PropertyDrawConditionAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
