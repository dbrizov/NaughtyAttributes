using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNonSerializedFieldTest : MonoBehaviour
    {
#pragma warning disable 414
        [ShowNonSerializedField]
        private ushort myUShort = ushort.MaxValue;

        [ShowNonSerializedField]
        private short myShort = short.MaxValue;

        [ShowNonSerializedField]
        private uint myUInt = uint.MaxValue;

        [ShowNonSerializedField]
        private int myInt = 10;

        [ShowNonSerializedField]
        private ulong myULong = ulong.MaxValue;

        [ShowNonSerializedField]
        private long myLong = long.MaxValue;

        [ShowNonSerializedField]
        private const float PI = 3.14159f;

        [ShowNonSerializedField]
        private static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414
    }
}
