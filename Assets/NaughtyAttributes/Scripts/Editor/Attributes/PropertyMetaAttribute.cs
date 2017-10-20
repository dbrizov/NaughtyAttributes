using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyMetaAttribute : BasePropertyAttribute
    {
        public PropertyMetaAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
