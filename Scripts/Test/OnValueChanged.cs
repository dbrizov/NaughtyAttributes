using UnityEngine;

public class OnValueChanged : MonoBehaviour
{
    [NaughtyAttributes.OnValueChanged("OnValueChangedMethod")]
    public int onValueChanged;

    private void OnValueChangedMethod()
    {
        Debug.Log(this.onValueChanged);
    }
}
