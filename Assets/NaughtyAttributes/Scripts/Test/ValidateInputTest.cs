using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ValidateInputTest : MonoBehaviour
    {
        [ValidateInput("NotZero0", "int0 must not be zero")]
        public int int0;

        private bool NotZero0(int value)
        {
            return value != 0;
        }

        public ValidateInputNest1 nest1;

        public ValidateInputInheritedNest inheritedNest;
    }

    [System.Serializable]
    public class ValidateInputNest1
    {
        [ValidateInput("NotZero1")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int int1;

        private bool NotZero1(int value)
        {
            return value != 0;
        }

        public ValidateInputNest2 nest2;
    }

    [System.Serializable]
    public class ValidateInputNest2
    {
        [ValidateInput("NotZero2")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int int2;

        private bool NotZero2(int value)
        {
            return value != 0;
        }
    }

    [System.Serializable]
    public class ValidateInputInheritedNest : ValidateInputNest1
    {
    }
}
