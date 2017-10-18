using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class HideIfAttribute : DrawerAttribute
    {
        private string conditionName;

        public HideIfAttribute(string conditionName)
        {
            this.conditionName = conditionName;
        }

        public string ConditionName
        {
            get
            {
                return this.conditionName;
            }
        }
    }
}
