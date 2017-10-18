using System;

namespace NaughtyAttributes.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PropertyGrouperAttribute : Attribute
    {
        private Type targetAttributeType;

        public PropertyGrouperAttribute(Type targetAttributeType)
        {
            this.targetAttributeType = targetAttributeType;
        }

        public Type TargetAttributeType
        {
            get
            {
                return this.targetAttributeType;
            }
        }
    }
}
