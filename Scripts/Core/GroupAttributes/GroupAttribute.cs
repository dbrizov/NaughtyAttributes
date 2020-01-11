using System;

namespace NaughtyAttributes
{
	public abstract class GroupAttribute : NaughtyAttribute
	{
		public string Name { get; private set; }

		public GroupAttribute(string name)
		{
			Name = name;
		}
	}
}
