using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class TagTest : MonoBehaviour
    {
        [Tag]
        public string tag0;

        public TagNest1 nest1;

        [Button]
        private void LogTag0()
        {
            Debug.Log(tag0);
        }
    }

    [System.Serializable]
    public class TagNest1
    {
        [Tag]
        public string tag1;

        public TagNest2 nest2;
    }

    [System.Serializable]
    public struct TagNest2
    {
        [Tag]
        public string tag2;
    }
}
