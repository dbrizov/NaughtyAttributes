using System;

namespace NaughtyAttributes
{
	public abstract class EnableIfAttributeBase : EnabledAttribute
	{
		// TODO: This is a provisional setter, once the meta attributes have been reworked, this field will be set
		//       from the editor assembly
		public override bool Enabled => true;
		public string[] Conditions { get; private set; }
		public EConditionOperator ConditionOperator { get; private set; }
		public bool Inverted { get; protected set; }

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
