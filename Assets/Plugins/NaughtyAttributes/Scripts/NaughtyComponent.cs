using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public List<Transform> transforms;

    [ShowNativeProperty]
    public string TransformsCount
    {
        get
        {
            return this.transforms.Count.ToString() + " Count";
        }
    }
}
