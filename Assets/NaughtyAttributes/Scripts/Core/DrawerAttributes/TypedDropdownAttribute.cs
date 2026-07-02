#if UNITY_2021_3_OR_NEWER
using UnityEngine;
using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Draws a dropdown with all types derived from the given base type
    /// </summary>
    public class TypeDropdownAttribute : PropertyAttribute
    {
        public readonly Type baseType;
        public readonly Type defaultType;
        public TypeDropdownAttribute(Type baseType) : this(baseType, baseType)
        {

        }

        public TypeDropdownAttribute(Type baseType, Type defaultType)
        {
            this.baseType = baseType;
            this.defaultType = defaultType;
        }
    }
}
#endif