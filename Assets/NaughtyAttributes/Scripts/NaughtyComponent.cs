using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public bool show;

    [MinMaxSlider(0f, 100f)]
    [ValidateInput("ValidateSlider")]
    public Vector2 slider;

    [Required]
    public Transform trans;

    private bool ValidateSlider(Vector2 slider)
    {
        return slider.magnitude > 0f;
    }
}
