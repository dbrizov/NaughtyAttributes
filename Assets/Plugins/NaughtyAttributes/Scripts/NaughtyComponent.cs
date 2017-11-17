using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public List<Transform> transforms;

    [ShowNativeProperty]
    public string StringProperty
    {
        get
        {
            return this.transforms.Count.ToString() + " Count";
        }
    }

    [ShowNativeProperty]
    public List<float> FloatsProperty
    {
        get
        {
            return null;
        }
    }

    [ShowNativeProperty]
    public Transform TransProperty
    {
        get
        {
            return this.transform;
        }
    }
}
