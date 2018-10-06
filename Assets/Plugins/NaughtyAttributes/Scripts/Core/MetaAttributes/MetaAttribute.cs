using System;

namespace NaughtyAttributes
{
    public abstract class MetaAttribute : NaughtyAttribute
    {
        public int Order { get; set; }
    }
}
