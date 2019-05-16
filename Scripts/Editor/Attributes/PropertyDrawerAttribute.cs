using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyDrawerAttribute : BaseAttribute
    {
        public PropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
