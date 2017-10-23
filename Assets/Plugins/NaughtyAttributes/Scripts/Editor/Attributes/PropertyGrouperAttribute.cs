using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyGrouperAttribute : BaseAttribute
    {
        public PropertyGrouperAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
