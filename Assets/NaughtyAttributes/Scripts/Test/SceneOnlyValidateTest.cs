using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class SceneOnlyValidateTest : MonoBehaviour
    {
        [SceneOnly]
        public GameObject reference;
        public SceneOnlyValidateNest1 nest1;
    }
    [System.Serializable]
    public class SceneOnlyValidateNest1
    {
        [SceneOnly]
        [AllowNesting]
        public GameObject reference;
        public SceneOnlyValidateNest2 nest2;
    }

    [System.Serializable]
    public class SceneOnlyValidateNest2
    {
        [SceneOnly]
        [AllowNesting]
        public GameObject reference;
    }
}
