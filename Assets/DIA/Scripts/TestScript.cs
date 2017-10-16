using UnityEngine;

public class TestScript : MonoBehaviour
{
    [MinValue(-100f), MaxValue(100f)]
    public float floatValue = 0f;
}
