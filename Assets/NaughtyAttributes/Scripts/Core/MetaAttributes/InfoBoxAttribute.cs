using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class InfoBoxAttribute : MetaAttribute
    {
        public string Text { get; private set; }
        public InfoBoxType Type { get; private set; }
        public string VisibleIf { get; private set; }

        public InfoBoxAttribute(string text, InfoBoxType type = InfoBoxType.Normal, string visibleIf = null)
        {
            this.Text = text;
            this.Type = type;
            this.VisibleIf = visibleIf;
        }

        public InfoBoxAttribute(string text, string visibleIf)
            : this(text, InfoBoxType.Normal, visibleIf)
        {
        }
    }

    public enum InfoBoxType
    {
        Normal,
        Warning,
        Error
    }
}
