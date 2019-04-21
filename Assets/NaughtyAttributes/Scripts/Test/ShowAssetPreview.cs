using UnityEngine;

public class ShowAssetPreview : MonoBehaviour
{
    [NaughtyAttributes.ShowAssetPreview]
    public Sprite sprite;

    [NaughtyAttributes.ShowAssetPreview(96, 96)]
    public GameObject prefab;
}
