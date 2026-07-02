#if UNITY_2021_3_OR_NEWER
using System;
using UnityEditor;

namespace NaughtyAttributes
{

    [CustomPropertyDrawer(typeof(TypeDropdownAttribute))]
    public class TypeDropdownAttributePropertyDrawer : SerializedReferencePropertyDrawerBase
    {
        protected override Type DefaultType => (attribute as TypeDropdownAttribute).defaultType;
        protected override Type BaseType => (attribute as TypeDropdownAttribute).baseType;
    }
}
#endif
