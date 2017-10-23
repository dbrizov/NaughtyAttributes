using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        private string text;

        public ButtonAttribute(string text = null)
        {
            this.text = text;
        }

        public string Text
        {
            get
            {
                return this.text;
            }
        }
    }
}
