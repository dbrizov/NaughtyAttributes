using System;

namespace NaughtyAttributes.Editor
{
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
	public abstract class BaseAttribute : Attribute, IAttribute
	{
		private Type _targetAttributeType;

		public BaseAttribute(Type targetAttributeType)
		{
			_targetAttributeType = targetAttributeType;
		}

		public Type TargetAttributeType
		{
			get
			{
				return _targetAttributeType;
			}
		}
	}
}
