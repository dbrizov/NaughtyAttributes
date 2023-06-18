using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class VarLabelAttribute : LabelAttribute
    {
        public VarLabelAttribute(string label) : base(label)
        {
        }
    }
}