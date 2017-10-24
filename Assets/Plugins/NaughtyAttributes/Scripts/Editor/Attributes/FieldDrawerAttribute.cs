using System;

namespace NaughtyAttributes.Editor
{
    public class FieldDrawerAttribute : BaseAttribute
    {
        public FieldDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
