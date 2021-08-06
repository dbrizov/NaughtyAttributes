using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class ExpandableAttribute : DrawerAttribute
	{
		public readonly bool IsWritable;
		public ExpandableAttribute(bool writable = true)
		{
			IsWritable = writable;
		}
	}
}
