using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class ReorderableLists : MonoBehaviour
{
    [BoxGroup("Reorderable Lists")]
    [ReorderableList]
    public int[] intArray;

    [BoxGroup("Reorderable Lists")]
    [ReorderableList]
    public List<Vector3> vectorList;
}
