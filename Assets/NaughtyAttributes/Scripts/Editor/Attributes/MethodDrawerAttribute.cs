using System;

namespace NaughtyAttributes.Editor
{
    public class MethodDrawerAttribute : BaseAttribute
    {
        public MethodDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
