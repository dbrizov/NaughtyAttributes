using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [ShowAssetPreview]
    public Sprite sprite;
    
    [ShowAssetPreview(128, 128)]
    public GameObject prefab;
}
