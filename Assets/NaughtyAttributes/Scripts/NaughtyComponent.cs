using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [MinMaxSlider(0f, 100f)]
    public Vector2 slider;
}
