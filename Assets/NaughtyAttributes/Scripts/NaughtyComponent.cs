using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    public int number;

    [Header("Header")]
    public string text;
    
    [Space]
    public Vector3 vector;
}
