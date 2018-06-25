using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class EnableIfAttribute : DrawerAttribute
    {
        private string conditionName;

        public EnableIfAttribute(string conditionName)
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
