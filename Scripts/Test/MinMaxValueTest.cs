using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class MinMaxValueTest : MonoBehaviour
    {
        // Section A — every clamp type with literal bounds.
        [MinValue(0), MaxValue(10)]
        [Label("Int Range [0, 10]")]
        public int intRange;

        [MinValue(0f), MaxValue(1f)]
        [Label("Float Range [0, 1]")]
        public float floatRange;

        [MinValue(0), MaxValue(1)]
        [Label("Vector2 Range [0, 1]")]
        public Vector2 vector2Range;

        [MinValue(0), MaxValue(1)]
        [Label("Vector3 Range [0, 1]")]
        public Vector3 vector3Range;

        [MinValue(0), MaxValue(1)]
        [Label("Vector4 Range [0, 1]")]
        public Vector4 vector4Range;

        [MinValue(0), MaxValue(10)]
        [Label("Vector2Int Range [0, 10]")]
        public Vector2Int vector2IntRange;

        [MinValue(0), MaxValue(10)]
        [Label("Vector3Int Range [0, 10]")]
        public Vector3Int vector3IntRange;

        // Section B — every bound-source kind, on a float.
        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Float Range [0, 1] (by field name)")]
        public float floatFromField;

        [MinValue(nameof(MinFloatProperty)), MaxValue(nameof(MaxFloatProperty))]
        [Label("Float Range [0, 1] (by property name)")]
        public float floatFromProperty;

        [MinValue(nameof(GetMinFloat)), MaxValue(nameof(GetMaxFloat))]
        [Label("Float Range [0, 1] (by method name)")]
        public float floatFromMethod;

        // Section C — by-name on each vector type.
        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Vector2 Range [0, 1] (by field name)")]
        public Vector2 vector2ByName;

        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Vector3 Range [0, 1] (by field name)")]
        public Vector3 vector3ByName;

        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Vector4 Range [0, 1] (by field name)")]
        public Vector4 vector4ByName;

        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Vector2Int Range [0, 1] (by field name)")]
        public Vector2Int vector2IntByName;

        [MinValue(nameof(minFloatField)), MaxValue(nameof(maxFloatField))]
        [Label("Vector3Int Range [0, 1] (by field name)")]
        public Vector3Int vector3IntByName;

        public MinMaxValueNest1 nest1;

#pragma warning disable CS0414
        private float minFloatField = 0f;
        private float maxFloatField = 1f;
#pragma warning restore CS0414

        private float MinFloatProperty => 0f;
        private float MaxFloatProperty => 1f;
        private float GetMinFloat() => 0f;
        private float GetMaxFloat() => 1f;
    }

    [System.Serializable]
    public class MinMaxValueNest1
    {
        [MinValue(0), MaxValue(10)]
        [Label("Int Range [0, 10]")]
        [AllowNesting]
        public int intRange;

        [MinValue(0f), MaxValue(1f)]
        [Label("Float Range [0, 1]")]
        [AllowNesting]
        public float floatRange;

        [MinValue(0), MaxValue(1)]
        [Label("Vector3 Range [0, 1]")]
        [AllowNesting]
        public Vector3 vector3Range;

        [MinValue(0), MaxValue(10)]
        [Label("Vector3Int Range [0, 10]")]
        [AllowNesting]
        public Vector3Int vector3IntRange;

        public MinMaxValueNest2 nest2;
    }

    [System.Serializable]
    public class MinMaxValueNest2
    {
        [MinValue(0), MaxValue(10)]
        [Label("Int Range [0, 10]")]
        [AllowNesting]
        public int intRange;

        [MinValue(0f), MaxValue(1f)]
        [Label("Float Range [0, 1]")]
        [AllowNesting]
        public float floatRange;

        [MinValue(0), MaxValue(1)]
        [Label("Vector3 Range [0, 1]")]
        [AllowNesting]
        public Vector3 vector3Range;

        [MinValue(0), MaxValue(10)]
        [Label("Vector3Int Range [0, 10]")]
        [AllowNesting]
        public Vector3Int vector3IntRange;
    }
}
