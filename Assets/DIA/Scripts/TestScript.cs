using UnityEngine;

public class TestScript : MonoBehaviour
{
    [ShowIf("asd")]
    [MinValue(-100f), MaxValue(100f)]
    public float floatValue = 0f;
}
