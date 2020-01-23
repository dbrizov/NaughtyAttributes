using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class AssetsOnlyValidateTest : MonoBehaviour
    {
        [AssetsOnly]
        public GameObject reference;
        public AssetsOnlyValidateNest1 nest1;
    }
    [System.Serializable]
    public class AssetsOnlyValidateNest1
    {
        [AssetsOnly]
        [AllowNesting]
        public GameObject reference;
        public AssetsOnlyValidateNest2 nest2;
    }

    [System.Serializable]
    public class AssetsOnlyValidateNest2
    {
        [AssetsOnly]
        [AllowNesting]
        public GameObject reference;
    }
}
