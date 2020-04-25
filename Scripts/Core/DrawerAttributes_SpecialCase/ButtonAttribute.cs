using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : SpecialCaseDrawerAttribute
	{
		public string Text { get; private set; }
		public EnableMode SelectedEnableMode { get; private set; }

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

		public ButtonAttribute(string text = null, EnableMode enabledMode = EnableMode.Always)
		{
			this.Text = text;
			this.SelectedEnableMode = enabledMode;
		}
	}
}
