using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute
    {
        private float minValue;

        public MinValueAttribute(float minValue)
        {
            this.minValue = minValue;
        }

        public MinValueAttribute(int minValue)
        {
            this.minValue = minValue;
        }

        public float MinValue
        {
            get
            {
                return this.minValue;
            }
        }
    }
}
