using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class MinMaxValueTest : MonoBehaviour
    {
        [MinValue(-10)]
        public int min0Int;

        [MaxValue(10)]
        public int max0Int;

        [MaxValue(nameof(max0Int))]
        public int maxByNameInt;

        [MinValue(nameof(min0Int))]
        public int minByNameInt;

        [MinValue(0), MaxValue(1)]
        public float range01Float;

        [MinValue(-1), MaxValue(0)]
        public float rangeMinus10Float;

        [MinValue(nameof(rangeMinus10Float)), MaxValue(nameof(range01Float))]
        public float rangeByNameFloat;

        [MinValue(0), MaxValue(1)]
        public Vector2 range01Vector2;

        [MinValue(nameof(rangeMinus10Float)), MaxValue(nameof(range01Float))]
        public Vector2 rangeByNameVector2;

        [MinValue(0), MaxValue(1)]
        public Vector3 range01Vector3;

        [MinValue(nameof(rangeMinus10Float)), MaxValue(nameof(range01Float))]
        public Vector3 rangeByNameVector3;

        [MinValue(0), MaxValue(1)]
        public Vector4 range01Vector4;

        [MinValue(nameof(rangeMinus10Float)), MaxValue(nameof(range01Float))]
        public Vector4 rangeByNameVector4;

        [MinValue(0)]
        public Vector2Int min0Vector2Int;

        [MaxValue(100)]
        public Vector2Int max100Vector2Int;

        [MaxValue(nameof(max0Int))]
        public Vector2Int maxByNameVector2Int;

        [MinValue(0)]
        public Vector3Int min0Vector3Int;

        [MaxValue(100)]
        public Vector3Int max100Vector3Int;

        [MaxValue(nameof(max0Int))]
        public Vector3Int maxByNameVector3Int;

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
