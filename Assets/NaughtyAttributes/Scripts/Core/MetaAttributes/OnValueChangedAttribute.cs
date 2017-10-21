using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true, Inherited = true)]
    public class OnValueChangedAttribute : MetaAttribute
    {
        private string callbackName;

        public OnValueChangedAttribute(string callbackName)
        {
            this.callbackName = callbackName;
        }

        public string CallbackName
        {
            get
            {
                return this.callbackName;
            }
        }
    }
}
