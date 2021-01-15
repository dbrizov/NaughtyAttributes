using System;

namespace NaughtyAttributes
{
	public enum EButtonEnableMode
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

	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : SpecialCaseDrawerAttribute
	{
		public string Text { get; private set; }
		public EButtonEnableMode SelectedEnableMode { get; private set; }

		public ButtonAttribute()
		{
			this.Text = null;
			this.SelectedEnableMode = EButtonEnableMode.Always;
		}
		public ButtonAttribute(string text)
		{
			this.Text = text;
			this.SelectedEnableMode = EButtonEnableMode.Always;
		}

		[Obsolete("enabledMode is obsolete, use [Button, EnabledInEditMode]")]
		public ButtonAttribute(EButtonEnableMode enabledMode)
		{
			this.Text = null;
			this.SelectedEnableMode = enabledMode;
		}
		
		[Obsolete("enabledMode is obsolete, use [Button, EnabledInEditMode]")]
		public ButtonAttribute(string text, EButtonEnableMode enabledMode)
		{
			this.Text = text;
			this.SelectedEnableMode = enabledMode;
		}
	}
}
