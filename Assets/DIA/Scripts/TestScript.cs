using UnityEngine;

public class TestScript : MonoBehaviour
{
    [SerializeField]
    private bool showFloatValue = true;

    [SerializeField]
    [ShowIf("showFloatValue")]
    [MinValue(-100f), MaxValue(100f)]
    private float floatValue = 0f;
}
