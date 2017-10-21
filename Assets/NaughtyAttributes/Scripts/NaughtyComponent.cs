using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public bool show;

    [MinMaxSlider(0f, 100f)]
    [OnValueChanged("OnValueChanged1")]
    [OnValueChanged("OnValueChanged2")]
    public Vector2 slider;

    public void OnValueChanged1()
    {
        Debug.Log("OnValueChanged1");
    }

    public void OnValueChanged2()
    {
        Debug.Log("OnValueChanged2");
    }
}
