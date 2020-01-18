using System;

namespace NaughtyAttributes
{
	public enum EInfoBoxType
	{
		Normal,
		Warning,
		Error
	}

	[AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
	public class InfoBoxAttribute : DrawerAttribute
	{
		public string Text { get; private set; }
		public EInfoBoxType Type { get; private set; }

		public InfoBoxAttribute(string text, EInfoBoxType type = EInfoBoxType.Normal)
		{
			Text = text;
			Type = type;
		}
	}
}
