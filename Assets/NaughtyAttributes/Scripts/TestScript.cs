using NaughtyAttributes;
using UnityEngine;

public class TestScript : MonoBehaviour
{    
    [BoxGroup("Group A")]
    public bool showFloatValue = true;

    [BoxGroup("Group A")]
    [ShowIf("showFloatValue")]
    [MinValue(-100f), MaxValue(100f)]
    public float floatValue = 0f;
    
    [BoxGroup("Group B")]
    public int intB;

    [BoxGroup("Group B")]
    public float floatB;
    
    public int intNoGroup;
    
    public float floatNoGroup;
}
