using System;

namespace NaughtyAttributes.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public abstract class BasePropertyAttribute : Attribute, IPropertyAttribute
    {
        private Type targetAttributeType;

        public BasePropertyAttribute(Type targetAttributeType)
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
