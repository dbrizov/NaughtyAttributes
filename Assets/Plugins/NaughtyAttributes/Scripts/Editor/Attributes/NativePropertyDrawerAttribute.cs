using System;

namespace NaughtyAttributes.Editor
{
    public class NativePropertyDrawerAttribute : BaseAttribute
    {
        public NativePropertyDrawerAttribute(Type targetAttributeType) : base(targetAttributeType)
        {
        }
    }
}
