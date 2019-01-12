using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class HideIfAnyAttribute : DrawConditionAttribute
    {
        public string ConditionName { get; private set; }

        public bool ConditionValue { get; private set; }

        public HideIfAnyAttribute(string conditionName, bool conditionValue = true)
        {
            this.ConditionName = conditionName;
            this.ConditionValue = conditionValue;
        }
    }
}
