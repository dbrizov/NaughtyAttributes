using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [Slider(0, 100)]
    public int slider1;
    
    [Slider(0f, 100f)]
    public float slider2;
}
