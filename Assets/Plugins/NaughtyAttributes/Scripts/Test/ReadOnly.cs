using UnityEngine;

public class ReadOnly : MonoBehaviour
{
    [NaughtyAttributes.ReadOnly]
    public int readOnlyInt = 5;
}
