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
        public EColor HighColor { get; private set;}
        public object HighValue { get; set; }
        public EColor LowColor { get; private set;}
        public object LowValue { get; set; }

        public ProgressBarAttribute(string name, float maxValue, EColor color, object highValue, EColor highColor,
            object lowValue, EColor lowColor)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;

            HighValue = highValue;
            HighColor = highColor;
            LowValue = lowValue;
            LowColor = lowColor;
        }
        public ProgressBarAttribute(string name, string maxValueName, EColor color, object highValue, EColor highColor,
            object lowValue, EColor lowColor)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;

            HighValue = highValue;
            HighColor = highColor;
            LowValue = lowValue;
            LowColor = lowColor;
        }

        public ProgressBarAttribute(string name, string maxValueName, EColor color, object highValue, EColor highColor)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;

            HighValue = highValue;
            HighColor = highColor;
        }
        
        public ProgressBarAttribute(string name, float maxValue, EColor color, object highValue, EColor highColor)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;

            HighValue = highValue;
            HighColor = highColor;
        }
        
        public ProgressBarAttribute(string name, float maxValue, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValue = maxValue;
            Color = color;
            
            HighColor = color; 
            LowColor = color;
        }

        public ProgressBarAttribute(string name, string maxValueName, EColor color = EColor.Blue)
        {
            Name = name;
            MaxValueName = maxValueName;
            Color = color;
            
            HighColor = color; 
            LowColor = color;
        }

        public ProgressBarAttribute(float maxValue, EColor color = EColor.Blue)
            : this("", maxValue, color)
        {
        }

        public ProgressBarAttribute(string maxValueName, EColor color = EColor.Blue)
            : this("", maxValueName, color)
        {
        }
    }
}
