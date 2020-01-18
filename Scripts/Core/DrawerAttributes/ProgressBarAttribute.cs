using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ProgressBarAttribute : DrawerAttribute
	{
		public string Name { get; private set; }
		public float MaxValue { get; private set; }
		public EColor Color { get; private set; }

		public ProgressBarAttribute(string name = "", float maxValue = 100, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValue = maxValue;
			Color = color;
		}
	}
}
