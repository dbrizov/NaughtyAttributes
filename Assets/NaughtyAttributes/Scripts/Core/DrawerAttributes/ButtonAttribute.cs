using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonAttribute : DrawerAttribute
    {
        public enum EnableMode
        {
            /// <summary>
            /// Button should be active always
            /// </summary>
            Always,
            /// <summary>
            /// Button should be active only in editor
            /// </summary>
            Editor,
            /// <summary>
            /// Button should be active only in playmode
            /// </summary>
            Playmode
        }

        public string Text { get; private set; }
        public EnableMode SelectedEnableMode { get; private set; }

        /// <param name="editorActive">Should the button be shown in editor?</param>
        /// <param name="runtimeActive">Should the button be shown when application is playing</param>
        public ButtonAttribute(string text = null, EnableMode enabledMode = EnableMode.Always)
        {
            this.Text = text;
            this.SelectedEnableMode = enabledMode;
        }
    }
}
