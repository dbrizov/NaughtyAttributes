using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : ShowIfAttribute
    {
        public HideIfAttribute(string condition)
            : base(condition)
        {
            Reversed = true;
        }

        public HideIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
            : base(conditionOperator, conditions)
        {
            Reversed = true;
        }
    }
}
