using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public bool show;

    [InfoBox("This is a slider", "show")]
    [Slider(0, 100)]
    public int slider;

    private bool AlwaysShow()
    {
        return true;
    }

    private bool NeverShow()
    {
        return false;
    }
}
