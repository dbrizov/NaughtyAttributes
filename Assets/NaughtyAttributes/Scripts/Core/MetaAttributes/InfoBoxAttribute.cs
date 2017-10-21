using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class InfoBoxAttribute : MetaAttribute
    {
        private string text;
        private InfoBoxType type;
        private string visibleIf;

        public InfoBoxAttribute(string text, InfoBoxType type = InfoBoxType.Normal, string visibleIf = null)
        {
            this.text = text;
            this.type = type;
            this.visibleIf = visibleIf;
        }

        public InfoBoxAttribute(string text, string visibleIf)
            : this(text, InfoBoxType.Normal, visibleIf)
        {
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }

        public InfoBoxType Type
        {
            get
            {
                return this.type;
            }
        }

        public string VisibleIf
        {
            get
            {
                return this.visibleIf;
            }
        }
    }

    public enum InfoBoxType
    {
        Normal,
        Warning,
        Error
    }
}
