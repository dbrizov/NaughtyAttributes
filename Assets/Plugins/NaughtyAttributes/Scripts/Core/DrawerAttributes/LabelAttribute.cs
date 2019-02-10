using System;
using UnityEngine;

namespace NaughtyAttributes
{
    /// <summary>
    /// Override default label
    /// </summary>
    [AttributeUsage(AttributeTargets.Field,
    AllowMultiple = false, Inherited = true)]
    public class LabelAttribute : DrawerAttribute
    {
        public string label { get; private set; }
        public LabelAttribute(string label)
        {
            this.label = label;
        }
    }

}
