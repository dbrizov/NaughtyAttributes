using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ShowIfAttribute : DrawConditionAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }

        public ShowIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
        }

        public ShowIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }
    }
}
