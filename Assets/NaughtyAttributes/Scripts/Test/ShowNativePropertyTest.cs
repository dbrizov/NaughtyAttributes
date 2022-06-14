using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class ShowNativePropertyTest : MonoBehaviour
    {
        [ShowNativeProperty]
        private Transform Transform
        {
            get
            {
                return transform;
            }
        }

        [ShowNativeProperty]
        private Transform ParentTransform
        {
            get
            {
                return transform.parent;
            }
        }

        [ShowNativeProperty]
        private ushort MyUShort
        {
            get
            {
                return ushort.MaxValue;
            }
        }

        [ShowNativeProperty]
        private short MyShort
        {
            get
            {
                return short.MaxValue;
            }
        }

        [ShowNativeProperty]
        private ulong MyULong
        {
            get
            {
                return ulong.MaxValue;
            }
        }

        [ShowNativeProperty]
        private long MyLong
        {
            get
            {
                return long.MaxValue;
            }
        }

        [ShowNativeProperty]
        private uint MyUInt
        {
            get
            {
                return uint.MaxValue;
            }
        }

        [ShowNativeProperty]
        private int MyInt
        {
            get
            {
                return int.MaxValue;
            }
        }
    }
}
