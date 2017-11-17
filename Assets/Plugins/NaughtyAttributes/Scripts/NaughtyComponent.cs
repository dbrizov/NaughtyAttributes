using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public List<Transform> transforms;

    [ShowNativeProperty]
    public int TransformsCount
    {
        get
        {
            return this.transforms.Count;
        }
    }
}
