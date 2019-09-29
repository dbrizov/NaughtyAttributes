using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ProgressBarAttribute : DrawerAttribute
    {
        public string Name { get; private set; }
        public float MaxValue { get; private set; }
        public ProgressBarColor Color { get; private set; }
        public string MaxValueFieldName { get; private set; }

        public ProgressBarAttribute(string name = "", string maxValueFieldName = "", float maxValue = 100, ProgressBarColor color = ProgressBarColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            MaxValueFieldName = maxValueFieldName;
            Color = color;
        }
    }

    public enum ProgressBarColor
    {
        Red,
        Pink,
        Orange,
        Yellow,
        Green,
        Blue,
        Indigo,
        Violet,
        White
    }
}