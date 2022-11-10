using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredTypeAttribute : ValidatorAttribute
    {
        public Type[] BaseTypes { get; private set; }
        
        public bool ShowInfoMessageWhenEmpty { get; private set; }
        
        public RequiredTypeAttribute(params Type[] baseTypes)
        {
            BaseTypes = baseTypes;
            ShowInfoMessageWhenEmpty = true;
        }
        
        public RequiredTypeAttribute(bool showInfoMessageWhenEmpty, params Type[] baseTypes)
        {
            BaseTypes = baseTypes;
            ShowInfoMessageWhenEmpty = showInfoMessageWhenEmpty;
        }
    }
}