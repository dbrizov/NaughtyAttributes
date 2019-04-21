using NaughtyAttributes;
using UnityEngine;

public class NonSerializedProperties : MonoBehaviour
{
#pragma warning disable 414
    [ShowNonSerializedField]
    private int myInt = 10;

    [ShowNonSerializedField]
    private const float PI = 3.14159f;

    [ShowNonSerializedField]
    private static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414

    [ShowNativeProperty]
    public Transform Transform
    {
        get
        {
            return this.transform;
        }
    }
}
