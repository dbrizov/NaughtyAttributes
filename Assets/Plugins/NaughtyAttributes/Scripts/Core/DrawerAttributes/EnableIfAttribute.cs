using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnableIfAttribute : DrawerAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }

        public EnableIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public EnableIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }
    }
}
