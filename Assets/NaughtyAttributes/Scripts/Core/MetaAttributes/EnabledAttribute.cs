using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public abstract class EnabledAttribute : MetaAttribute, IEnabledAttribute
    {
        public abstract bool Enabled { get; }
    }
}