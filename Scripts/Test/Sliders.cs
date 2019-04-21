using NaughtyAttributes;
using UnityEngine;

public class Sliders : MonoBehaviour
{
    [BoxGroup("Sliders")]
    [Slider(0, 10)]
    public int intSlider;

    [BoxGroup("Sliders")]
    [Slider(0.0f, 10.0f)]
    public float floatSlider;

    [BoxGroup("Sliders")]
    [MinMaxSlider(0.0f, 100.0f)]
    public Vector2 minMaxSlider;
}
