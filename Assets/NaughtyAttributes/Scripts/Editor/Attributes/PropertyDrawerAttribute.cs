using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyDrawerAttribute : BasePropertyAttribute
    {
        public PropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
