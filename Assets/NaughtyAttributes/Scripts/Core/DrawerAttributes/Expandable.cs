using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ExpandableAttribute : DrawerAttribute
    {
        public bool IsReadonly { get; private set; }

        public ExpandableAttribute(bool isReadonly = false)
        {
            IsReadonly = isReadonly;
        }
    }
}
