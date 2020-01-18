using System;

namespace NaughtyAttributes
{
	public abstract class EnableIfAttributeBase : Attribute, IMetaAttribute
	{
		public string[] Conditions { get; private set; }
		public EConditionOperator ConditionOperator { get; private set; }
		public bool Reversed { get; protected set; }

		public EnableIfAttributeBase(string condition)
		{
			ConditionOperator = EConditionOperator.And;
			Conditions = new string[1] { condition };
		}

		public EnableIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
		{
			ConditionOperator = conditionOperator;
			Conditions = conditions;
		}
	}
}
