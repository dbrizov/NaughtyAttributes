using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RequiredTypeAttribute : ValidatorAttribute
    {
        public Type[] RequiredTypes { get; private set; }
        
        public bool ShowInfoMessageWhenEmpty { get; private set; }
        
        public RequiredTypeAttribute(params Type[] requiredTypes)
        {
            RequiredTypes = requiredTypes;
            ShowInfoMessageWhenEmpty = true;
        }
        
        public RequiredTypeAttribute(bool showInfoMessageWhenEmpty, params Type[] requiredTypes)
        {
            RequiredTypes = requiredTypes;
            ShowInfoMessageWhenEmpty = showInfoMessageWhenEmpty;
        }
    }
}