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

    [BoxGroup("Group C")]
    public bool showLists = true;

    [BoxGroup("Group C")]
    [ShowIf("showLists")]
    [ReorderableList]
    public List<int> integers;

    [BoxGroup("Group C")]
    [ShowIf("showLists")]
    [ReorderableList]
    public float[] floats;
}
