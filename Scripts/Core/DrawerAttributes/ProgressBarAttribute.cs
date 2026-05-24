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
        public string ColorName { get; private set; }

        public ProgressBarAttribute(string name, float maxValue, EColor color = EColor.Blue)
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

        public ProgressBarAttribute(float maxValue, EColor color = EColor.Blue)
            : this("", maxValue, color)
        {
        }

        public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
            : this("", maxValueName, color)
        {
        }

        public ProgressBarAttribute(string maxValueName, string colorName)
            : this("", maxValueName, colorName)
        {
        }

        public ProgressBarAttribute(float maxValue, string colorName)
            : this("", maxValue, colorName)
        {
        }

        private ProgressBarAttribute(string name, string maxValueName, string colorName)
        {
            Name = name;
            MaxValueName = maxValueName;
            ColorName = colorName;
        }

        private ProgressBarAttribute(string name, float maxValue, string colorName)
        {
            Name = name;
            MaxValue = maxValue;
            ColorName = colorName;
        }
    }
}
