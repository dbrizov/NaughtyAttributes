using NaughtyAttributes;
using UnityEngine;

public class OnlyAssetsSceneObjectsTester : MonoBehaviour
{

    [SerializeField]
    [OnlyAssets]
    private GameObject _SomePrefab1;

    [SerializeField]
    [OnlyAssets(true)]
    private GameObject _SomePrefab2;

    [SerializeField]
    [OnlySceneObjects]
    private GameObject _SomeSceneObject1;

    [SerializeField]
    [OnlySceneObjects(true)]
    private GameObject _SomeSceneObject2;

}
