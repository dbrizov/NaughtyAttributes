using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SliderAttribute : DrawerAttribute
    {
        private float minValue;
        private float maxValue;

        public SliderAttribute(float minValue, float maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public SliderAttribute(int minValue, int maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public float MinValue
        {
            get
            {
                return this.minValue;
            }
        }

        public float MaxValue
        {
            get
            {
                return this.maxValue;
            }
        }
    }
}
