using System;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class EnableIfAttribute : EnableIfAttributeBase
	{
		public EnableIfAttribute(string condition)
			: base(condition)
		{
			Reversed = false;
		}

		public EnableIfAttribute(EConditionOperator conditionOperator, params string[] conditions)
			: base(conditionOperator, conditions)
		{
			Reversed = false;
		}
	}
}
