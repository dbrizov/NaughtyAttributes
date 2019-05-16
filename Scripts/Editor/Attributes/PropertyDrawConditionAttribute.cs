using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyDrawConditionAttribute : BaseAttribute
    {
        public PropertyDrawConditionAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
