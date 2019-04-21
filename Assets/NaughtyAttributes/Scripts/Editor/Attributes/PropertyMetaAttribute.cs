using System;

namespace NaughtyAttributes.Editor
{
    public class PropertyMetaAttribute : BaseAttribute
    {
        public PropertyMetaAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
