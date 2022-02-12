using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowAssetPreviewTest : MonoBehaviour
    {
        [ShowAssetPreview]
        public Sprite sprite0;

        [ShowAssetPreview(96, 96)]
        public GameObject prefab0;

        public ShowAssetPreviewNest1 nest1;
    }

    [System.Serializable]
    public class ShowAssetPreviewNest1
    {
        [ShowAssetPreview]
        public Sprite sprite1;

        [ShowAssetPreview(96, 96)]
        public GameObject prefab1;

        public ShowAssetPreviewNest2 nest2;
    }

    [System.Serializable]
    public class ShowAssetPreviewNest2
    {
        [ShowAssetPreview]
        public Sprite sprite2;

        [ShowAssetPreview(96, 96)]
        public GameObject prefab2;
    }
}
