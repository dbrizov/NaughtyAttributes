using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ProgressBarAttribute : DrawerAttribute
	{
		public string Name { get; private set; }
		public float MaxValue { get; set; }
		public string MaxValueName { get; private set; }
		public EColor Color { get; private set; }

		public ProgressBarAttribute(string name, int maxValue, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValue = maxValue;
			Color = color;
		}

		public ProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValueName = maxValueName;
			Color = color;
		}

		public ProgressBarAttribute(int maxValue, EColor color = EColor.Blue)
			: this("", maxValue, color)
		{
		}

		public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
			: this("", maxValueName, color)
		{
		}
	}
}
