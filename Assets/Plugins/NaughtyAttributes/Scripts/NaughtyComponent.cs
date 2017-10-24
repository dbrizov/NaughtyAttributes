using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [ShowNonSerializedField]
    private int myInt = 10;

    [ShowNonSerializedField]
    private const float PI = 3.14159f;

    [ShowNonSerializedField]
    private static readonly Vector3 CONST_VECTOR =
        new Vector3(1f, 1f, 1f);
}
