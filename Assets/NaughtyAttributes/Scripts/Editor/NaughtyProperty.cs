using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public class NaughtyProperty
    {
        public SerializedProperty serializedProperty;
        public SpecialCaseDrawerAttribute specialCaseDrawerAttribute;
        public ShowIfAttributeBase showIfAttribute;

        public EnableIfAttributeBase enableIfAttribute;

        public ReadOnlyAttribute readOnlyAttribute;

        public ValidatorAttribute[] validatorAttributes;
    }
}
