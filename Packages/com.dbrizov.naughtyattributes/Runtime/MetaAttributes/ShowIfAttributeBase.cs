using System;

namespace NaughtyAttributes
{
	public class ShowIfAttributeBase : ShowAttribute
	{
		// TODO: This is a provisional setter, once the meta attributes have been reworked, this field will be set
		//       from the editor assembly
		public override bool Visible => true;
		public string[] Conditions { get; private set; }
		public EConditionOperator ConditionOperator { get; private set; }
		public bool Inverted { get; protected set; }

		public ShowIfAttributeBase(string condition)
		{
			ConditionOperator = EConditionOperator.And;
			Conditions = new string[1] { condition };
		}

		public ShowIfAttributeBase(EConditionOperator conditionOperator, params string[] conditions)
		{
			ConditionOperator = conditionOperator;
			Conditions = conditions;
		}
	}
}
