using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReadOnlyAttribute : MetaAttribute
    {
        public bool IsEditableInEditMode;

        public ReadOnlyAttribute(){}

        public ReadOnlyAttribute(bool isEditableInEditMode)
        {
            IsEditableInEditMode = isEditableInEditMode;
        }
    }
}
