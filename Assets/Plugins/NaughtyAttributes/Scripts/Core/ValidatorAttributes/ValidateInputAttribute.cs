using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        public string CallbackName { get; private set; }
        public string Message { get; private set; }
        public bool HideWithField { get; private set; }

        public ValidateInputAttribute(string callbackName, string message = null, bool hideWithField = true)
        {
            this.CallbackName = callbackName;
            this.Message = message;
            this.HideWithField = hideWithField;
        }
    }
}
