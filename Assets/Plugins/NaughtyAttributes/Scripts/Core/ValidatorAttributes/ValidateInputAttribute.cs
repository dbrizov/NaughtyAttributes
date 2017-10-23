using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ValidateInputAttribute : ValidatorAttribute
    {
        private string callbackName;
        private string message;

        public ValidateInputAttribute(string callbackName, string message = null)
        {
            this.callbackName = callbackName;
            this.message = message;
        }

        public string CallbackName
        {
            get
            {
                return this.callbackName;
            }
        }

        public string Message
        {
            get
            {
                return this.message;
            }
        }
    }
}
