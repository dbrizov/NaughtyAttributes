using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute, IValuableAttribute
    {
        public string MaxValueName { get; private set; }
        public float MaxValue { get; private set; }

        public string ValueName => MaxValueName;
        public float Value => MaxValue;
        public bool IsDynamic => !string.IsNullOrEmpty(MaxValueName);

        public MaxValueAttribute(float maxValue)
        {
            MaxValue = maxValue;
        }

        public MaxValueAttribute(int maxValue)
        {
            MaxValue = maxValue;
        }

        public MaxValueAttribute(string maxValueName)
        {
            MaxValueName = maxValueName;
        }
    }
}
