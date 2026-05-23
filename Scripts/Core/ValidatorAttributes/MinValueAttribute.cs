using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MinValueAttribute : ValidatorAttribute, IValuableAttribute
    {
        public float MinValue { get; private set; }
        public string MinValueName { get; private set; }

        public string ValueName => MinValueName;
        public float Value => MinValue;
        public bool IsDynamic => !string.IsNullOrEmpty(MinValueName);

        public MinValueAttribute(float minValue)
        {
            MinValue = minValue;
        }

        public MinValueAttribute(int minValue)
        {
            MinValue = minValue;
        }

        public MinValueAttribute(string minValueName)
        {
            MinValueName = minValueName;
        }
    }
}
