using NaughtyAttributes;
using UnityEngine;

public class TestScript : MonoBehaviour
{    
    public bool showFloatValue = true;
    
    [ShowIf("showFloatValue")]
    [MinValue(-100f), MaxValue(100f)]
    public float floatValue = 0f;
    
    public int intB;
    
    public float floatB;

    public int intNoGroup;

    public float floatNoGroup;
}
