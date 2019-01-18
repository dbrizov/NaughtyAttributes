using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredAttribute : ValidatorAttribute
    {
        public string Message { get; private set; }
        public bool HideWithField { get; private set; }

        public RequiredAttribute(string message = null, bool hideWithField = true)
        {
            this.Message = message;
            this.HideWithField = hideWithField;
        }
    }
}
