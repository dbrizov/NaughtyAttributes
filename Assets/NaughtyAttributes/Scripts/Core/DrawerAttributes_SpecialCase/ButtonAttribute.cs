using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
	public class ButtonAttribute : SpecialCaseDrawerAttribute
	{
		public string Text { get; private set; }
		public float SpaceBefore = 8;
		public float SpaceAfter = 0;

		public ButtonAttribute(string text = null)
		{
			Text = text;
		}
	}
}
