using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class FoldoutAttribute : MetaAttribute
	{
		public string Name { get; private set; }

		public FoldoutAttribute(string name = "")
		{
			Name = name;
		}
	}
}
