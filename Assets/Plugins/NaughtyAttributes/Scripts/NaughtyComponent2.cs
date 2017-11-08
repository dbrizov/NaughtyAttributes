using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent2 : NaughtyComponent
{
    [MinMaxSlider(-10f, 10f)]
    public Vector2 minMaxSlider;
    
    [ReorderableList]
    public int[] list;

    [ShowNonSerializedField]
    private int nonSerialized2 = 2;

    [Button]
    private void Method2()
    {
        Debug.Log("2");
    }
}
