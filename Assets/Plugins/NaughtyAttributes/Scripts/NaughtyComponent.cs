using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [Slider(0, 10)]
    public int intSlider;
    
    [Slider(0f, 10f)]
    public float floatSlider;

    [ShowNonSerializedField]
    private int nonSerialized1 = 1;

    [Button]
    private void Method1()
    {
        Debug.Log("1");
    }
}
