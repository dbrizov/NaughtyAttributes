using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class LabelAttribute : MetaAttribute
	{
		public string Label { get; private set; }

		public LabelAttribute(string label)
		{
			Label = label;
		}
	}
}
