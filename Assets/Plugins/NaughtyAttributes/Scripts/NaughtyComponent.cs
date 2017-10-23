using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [ReorderableList]
    public int[] intArray;

    [ReorderableList]
    public List<float> floatArray;

    [Button]
    private bool InstanceMethod()
    {
        Debug.Log("Instance");
        return true;
    }

    [Button]
    private static void StaticMethod()
    {
        Debug.Log("Static");
    }
}
