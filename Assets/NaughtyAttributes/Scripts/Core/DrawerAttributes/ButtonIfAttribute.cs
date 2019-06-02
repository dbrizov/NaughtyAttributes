using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public class ButtonIfAttribute : DrawerAttribute
    {
        public string[] Conditions { get; private set; }
        public ConditionOperator ConditionOperator { get; private set; }
        public bool Reversed { get; protected set; }

        public string Text { get; private set; }

        public ButtonIfAttribute(string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            this.Text = null;
        }

        public ButtonIfAttribute(string text, string condition)
        {
            ConditionOperator = ConditionOperator.And;
            Conditions = new string[1] { condition };
            this.Text = text;
        }

        public ButtonIfAttribute(ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }

        public ButtonIfAttribute(string text, ConditionOperator conditionOperator, params string[] conditions)
        {
            ConditionOperator = conditionOperator;
            Conditions = conditions;
        }
    }
}
