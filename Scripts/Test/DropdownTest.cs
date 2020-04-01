using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
	public class DropdownTest : MonoBehaviour
	{
		        [System.Flags]
        public enum FlagsEnum
        {
            A = 0,
            B = 1,
            C = 2,
            D = 4,
            E = C | D,
        }

        [System.Serializable]
        public struct MegaValueType : System.IEquatable<MegaValueType>
        {
            public int[] arrayValue;
            public int intValue;
            public LayerMask layerMaskValue;
            public char charValue;
            public bool boolValue;
            public float floatValue;
            public string stringValue;
            public Color colorValue;
            public GameObject objectReferenceValue;
            public FlagsEnum enumValue;
            public Vector2 vector2Value;
            public Vector3 vector3Value;
            public Vector4 vector4Value;
            public Rect rectValue;
            public AnimationCurve animationCurveValue;
            public Bounds boundsValue;
            public Quaternion quaternionValue;
            public ExposedReference<GameObject> exposedReferenceValue;
            public Vector2Int vector2IntValue;
            public Vector3Int vector3IntValue;
            public RectInt rectIntValue;
            public BoundsInt boundsIntValue;
            public Gradient gradientValue;
            public NestedValueType genericValue;


            public bool Equals(MegaValueType other)
            {
                return intValue == other.intValue
                       && ((int) layerMaskValue).Equals(other.layerMaskValue)
                       && charValue == other.charValue
                       && boolValue == other.boolValue
                       && floatValue.Equals(other.floatValue)
                       && stringValue == other.stringValue
                       && colorValue.Equals(other.colorValue)
                       && Equals(objectReferenceValue, other.objectReferenceValue)
                       && enumValue == other.enumValue
                       && vector2Value.Equals(other.vector2Value)
                       && vector3Value.Equals(other.vector3Value)
                       && vector4Value.Equals(other.vector4Value)
                       && rectValue.Equals(other.rectValue)
                       && Equals(animationCurveValue, other.animationCurveValue)
                       && boundsValue.Equals(other.boundsValue)
                       && quaternionValue.Equals(other.quaternionValue)
                       && exposedReferenceValue.Equals(other.exposedReferenceValue)
                       && vector2IntValue.Equals(other.vector2IntValue)
                       && vector3IntValue.Equals(other.vector3IntValue)
                       && rectIntValue.Equals(other.rectIntValue)
                       && boundsIntValue.Equals(other.boundsIntValue)
                       && Equals(gradientValue, other.gradientValue)
                       && genericValue.Equals(other.genericValue)
                    ;
            }

            public override bool Equals(object obj)
            {
                return obj is MegaValueType other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = intValue;
                    hashCode = (hashCode * 397) ^ layerMaskValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ charValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ boolValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ floatValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ (stringValue != null ? stringValue.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ colorValue.GetHashCode();
                    hashCode = (hashCode * 397) ^
                               (objectReferenceValue != null ? objectReferenceValue.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ (int) enumValue;
                    hashCode = (hashCode * 397) ^ vector2Value.GetHashCode();
                    hashCode = (hashCode * 397) ^ vector3Value.GetHashCode();
                    hashCode = (hashCode * 397) ^ vector4Value.GetHashCode();
                    hashCode = (hashCode * 397) ^ rectValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ (animationCurveValue != null ? animationCurveValue.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ boundsValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ quaternionValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ exposedReferenceValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ vector2IntValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ vector3IntValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ rectIntValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ boundsIntValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ (gradientValue != null ? gradientValue.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ genericValue.GetHashCode();
                    return hashCode;
                }
            }
        }

        [System.Serializable]
        public struct NestedValueType : System.IEquatable<NestedValueType>
        {
            public int intValue;
            public LayerMask layerMaskValue;
            public AnimationCurve animationCurveValue;
            public Vector2Int vector2IntValue;

            public bool Equals(NestedValueType other)
            {
                return intValue == other.intValue
                       && layerMaskValue.Equals(other.layerMaskValue)
                       && Equals(animationCurveValue, other.animationCurveValue)
                       && vector2IntValue.Equals(other.vector2IntValue);
            }

            public override bool Equals(object obj)
            {
                return obj is NestedValueType other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    int hashCode = intValue;
                    hashCode = (hashCode * 397) ^ layerMaskValue.GetHashCode();
                    hashCode = (hashCode * 397) ^ (animationCurveValue != null ? animationCurveValue.GetHashCode() : 0);
                    hashCode = (hashCode * 397) ^ vector2IntValue.GetHashCode();
                    return hashCode;
                }
            }
        }

        public DropdownList<MegaValueType> GetValueTypes()
        {
            return new DropdownList<MegaValueType>
            {
                {
                    "Zero",
                    new MegaValueType
                    {
                        arrayValue = new []{ 0 },
                        intValue = 0,
                        layerMaskValue = 0,
                        charValue = 'a',
                        boolValue = false,
                        floatValue = 0,
                        stringValue = "a",
                        colorValue = Color.red,
                        objectReferenceValue = null,
                        enumValue = FlagsEnum.A | FlagsEnum.B,
                        vector2Value = Vector2.zero,
                        vector3Value = Vector3.zero,
                        vector4Value = Vector4.zero,
                        rectValue = Rect.zero,
                        animationCurveValue = AnimationCurve.EaseInOut(0, 0, 1, 1),
                        boundsValue = new Bounds(Vector3.zero, Vector3.zero),
                        quaternionValue = Quaternion.identity,
                        exposedReferenceValue = new ExposedReference<GameObject>(),
                        vector2IntValue = Vector2Int.zero,
                        vector3IntValue = Vector3Int.zero,
                        rectIntValue = new RectInt(Vector2Int.zero, Vector2Int.zero),
                        boundsIntValue = new BoundsInt(Vector3Int.zero, Vector3Int.zero),
                        gradientValue = new Gradient(),
                        genericValue = new NestedValueType
                        {
                            intValue = 0,
                            animationCurveValue = AnimationCurve.EaseInOut(0, 0, 1, 1),
                            layerMaskValue = 0,
                            vector2IntValue = Vector2Int.zero,
                        }
                    }
                },

                {
                    "One",
                    new MegaValueType
                    {
                        arrayValue = new []{ 1, 1 },
                        intValue = 1,
                        layerMaskValue = 1,
                        charValue = 'b',
                        boolValue = true,
                        floatValue = 1,
                        stringValue = "b",
                        colorValue = Color.green,
                        objectReferenceValue = gameObject,
                        enumValue = FlagsEnum.C | FlagsEnum.D,
                        vector2Value = Vector2.one,
                        vector3Value = Vector3.one,
                        vector4Value = Vector4.one,
                        rectValue = new Rect(Vector2.one, Vector2.one),
                        animationCurveValue = AnimationCurve.EaseInOut(0, 1, 1, 0),
                        boundsValue = new Bounds(Vector3.one, Vector3.one),
                        quaternionValue = Quaternion.Euler(90, 90, 90),
                        exposedReferenceValue = new ExposedReference<GameObject>{ defaultValue = gameObject },
                        vector2IntValue = Vector2Int.one,
                        vector3IntValue = Vector3Int.one,
                        rectIntValue = new RectInt(Vector2Int.one, Vector2Int.one),
                        boundsIntValue = new BoundsInt(Vector3Int.one, Vector3Int.one),
                        gradientValue = new Gradient(),
                        genericValue = new NestedValueType
                        {
                            intValue = 1,
                            animationCurveValue = AnimationCurve.EaseInOut(0, 1, 1, 0),
                            layerMaskValue = 1,
                            vector2IntValue = Vector2Int.one,
                        }
                    }
                }
            };
        }

        [Dropdown(nameof(GetValueTypes))]
        [OnValueChanged(nameof(OnValueChangedCallback))]
        public MegaValueType valueType;

        private void OnValueChangedCallback(MegaValueType oldValue, MegaValueType newValue)
        {
            copyType = valueType;
        }

        public MegaValueType copyType;
		
		[Dropdown("intValues")]
		public int intValue;

#pragma warning disable 414
		private int[] intValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

		public DropdownNest1 nest1;
	}

	[System.Serializable]
	public class DropdownNest1
	{
		[Dropdown("StringValues")]
		public string stringValue;

		private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

		public DropdownNest2 nest2;
	}

	[System.Serializable]
	public class DropdownNest2
	{
		[Dropdown("GetVectorValues")]
		public Vector3 vectorValue;

		private DropdownList<Vector3> GetVectorValues()
		{
			return new DropdownList<Vector3>()
			{
				{ "Right", Vector3.right },
				{ "Up", Vector3.up },
				{ "Forward", Vector3.forward }
			};
		}
	}
}
