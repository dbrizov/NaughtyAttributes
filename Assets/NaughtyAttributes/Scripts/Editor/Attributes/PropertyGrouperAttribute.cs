using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyGrouperAttribute : BasePropertyAttribute
    {
        public PropertyGrouperAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
