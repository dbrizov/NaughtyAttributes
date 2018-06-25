using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DisableIfAttribute : DrawerAttribute
    {
        private string conditionName;

        public DisableIfAttribute(string conditionName)
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
