using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class TypeAttribute : DrawerAttribute
    {
        public Type[] types;
        public TypeAttribute(params Type[] types)
        {
            this.types = types;
        }
    }
}