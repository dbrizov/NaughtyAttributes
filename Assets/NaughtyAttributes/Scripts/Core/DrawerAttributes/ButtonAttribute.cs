using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        public string Text { get; private set; }

        public bool activeInEditMode = true;
        public bool activeInPlayMode = true;

        public ButtonAttribute(string text = null)
        {
            this.Text = text;
        }
    }
}
