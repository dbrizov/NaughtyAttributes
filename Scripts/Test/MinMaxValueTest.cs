using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class MinMaxValueTest : MonoBehaviour
    {
        [MinValue(0)]
        public int min0Int;

        [MaxValue(0)]
        public int max0Int;

        [MinValue(0), MaxValue(1)]
        public float range01Float;

        [MinValue(0), MaxValue(1)]
        public Vector2 range01Vector2;

        [MinValue(0), MaxValue(1)]
        public Vector3 range01Vector3;

        [MinValue(0), MaxValue(1)]
        public Vector4 range01Vector4;

        [MinValue(0)]
        public Vector2Int min0Vector2Int;

        [MaxValue(100)]
        public Vector2Int max100Vector2Int;

        [MinValue(0)]
        public Vector3Int min0Vector3Int;

        [MaxValue(100)]
        public Vector3Int max100Vector3Int;

        public MinMaxValueNest1 nest1;
    }

    [System.Serializable]
    public class MinMaxValueNest1
    {
        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int min0Int;

        [MaxValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int max0Int;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public float range01Float;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2 range01Vector2;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3 range01Vector3;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector4 range01Vector4;

        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int min0Vector2Int;

        [MaxValue(100)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int max100Vector2Int;

        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int min0Vector3Int;

        [MaxValue(100)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int max100Vector3Int;

        public MinMaxValueNest2 nest2;
    }

    [System.Serializable]
    public class MinMaxValueNest2
    {
        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int min0Int;

        [MaxValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int max0Int;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public float range01Float;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2 range01Vector2;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3 range01Vector3;

        [MinValue(0), MaxValue(1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector4 range01Vector4;

        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int min0Vector2Int;

        [MaxValue(100)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector2Int max100Vector2Int;

        [MinValue(0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int min0Vector3Int;

        [MaxValue(100)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public Vector3Int max100Vector3Int;
    }
}
