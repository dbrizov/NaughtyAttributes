using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    [BoxGroup("Group A")]
    public bool showFloatValue = true;

    [BoxGroup("Group A")]
    [ShowIf("showFloatValue")]
    public float floatValue = 0f;

    [BoxGroup("Group B")]
    public int intB;

    [BoxGroup("Group B")]
    public float floatB;

    [ReorderableList]
    public List<int> integers;

    [ReorderableList]
    public float[] floats;
}
