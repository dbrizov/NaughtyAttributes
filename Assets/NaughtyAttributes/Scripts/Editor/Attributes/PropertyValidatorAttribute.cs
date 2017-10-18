using System;

namespace NaughtyAttributes.Editor
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class PropertyValidatorAttribute : Attribute
    {
        private Type targetAttributeType;

        public PropertyValidatorAttribute(Type targetAttributeType)
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
