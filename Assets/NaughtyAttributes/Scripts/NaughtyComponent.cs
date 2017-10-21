using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public bool show = true;

    [Slider(0, 100)]
    public int slider1;

    [Slider(0f, 100f)]
    public float slider2;
    
    [Section("Section")]
    [ShowIf("show")]
    [MinMaxSlider(-10f, 10f)]
    public Vector2 minMaxSlider;
    
    [Section("Other Section")]
    [MultiLineText]
    public string someString = "";
}
