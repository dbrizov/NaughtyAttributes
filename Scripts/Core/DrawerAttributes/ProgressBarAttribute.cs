using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ProgressBarAttribute : DrawerAttribute
	{
		public string Name { get; private set; }
		public float MaxValue { get; set; }
		public EColor Color { get; private set; }
		public string MaxValueFieldName { get; private set; }

		public ProgressBarAttribute(string name = "", string maxValueFieldName = "", int maxValue = 100, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValue = maxValue;
			MaxValueFieldName = maxValueFieldName;
			Color = color;
		}
		
		public ProgressBarAttribute(string name = "", int maxValue = 100, EColor color = EColor.Blue)
		{
			Name = name;
			MaxValue = maxValue;
			Color = color;
		}
	}
}
