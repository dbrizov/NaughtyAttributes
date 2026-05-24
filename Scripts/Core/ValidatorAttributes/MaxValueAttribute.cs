using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class MaxValueAttribute : ValidatorAttribute
    {
        public string MaxValueName { get; private set; }
        public float MaxValue { get; private set; }

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
