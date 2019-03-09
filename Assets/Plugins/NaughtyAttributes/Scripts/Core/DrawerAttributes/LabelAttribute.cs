using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Override default label
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : DrawerAttribute
    {
        public string Label { get; private set; }

        public LabelAttribute(string label)
        {
            this.Label = label;
        }
    }
}
