using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class SliderAttribute : DrawerAttribute
	{
		public float MinValue { get; private set; }
		public float MaxValue { get; private set; }

		public SliderAttribute(float minValue, float maxValue)
		{
			MinValue = minValue;
			MaxValue = maxValue;
		}

		public SliderAttribute(int minValue, int maxValue)
		{
			MaxValue = minValue;
			MaxValue = maxValue;
		}
	}
}
