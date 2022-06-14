using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HideIfTest : MonoBehaviour
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [EnumFlags] public HideIfEnumFlag enum2;

        [HideIf(EConditionOperator.And, "hide1", "hide2")]
        [ReorderableList]
        public int[] hideIfAll;

        [HideIf(EConditionOperator.Or, "hide1", "hide2")]
        [ReorderableList]
        public int[] hideIfAny;

        [HideIf("enum1", HideIfEnum.Case0)]
        [ReorderableList]
        public int[] hideIfEnum;

        [HideIf("enum2", HideIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] hideIfEnumFlag;

        [HideIf("enum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] hideIfEnumFlagMulti;

        public HideIfNest1 nest1;
    }

    [System.Serializable]
    public class HideIfNest1
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [EnumFlags] public HideIfEnumFlag enum2;
        public bool Hide1 { get { return hide1; } }
        public bool Hide2 { get { return hide2; } }
        public HideIfEnum Enum1 { get { return enum1; } }
        public HideIfEnumFlag Enum2 { get { return enum2; } }

        [HideIf(EConditionOperator.And, "Hide1", "Hide2")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfAll;

        [HideIf(EConditionOperator.Or, "Hide1", "Hide2")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfAny;

        [HideIf("Enum1", HideIfEnum.Case1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int hideIfEnum;

        [HideIf("Enum2", HideIfEnumFlag.Flag0)]
        [AllowNesting]
        public int hideIfEnumFlag;

        [HideIf("Enum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [AllowNesting]
        public int hideIfEnumFlagMulti;

        public HideIfNest2 nest2;
    }

    [System.Serializable]
    public class HideIfNest2
    {
        public bool hide1;
        public bool hide2;
        public HideIfEnum enum1;
        [EnumFlags] public HideIfEnumFlag enum2;
        public bool GetHide1() { return hide1; }
        public bool GetHide2() { return hide2; }
        public HideIfEnum GetEnum1() { return enum1; }
        public HideIfEnumFlag GetEnum2() { return enum2; }

        [HideIf(EConditionOperator.And, "GetHide1", "GetHide2")]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfAll = new Vector2(0.25f, 0.75f);

        [HideIf(EConditionOperator.Or, "GetHide1", "GetHide2")]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfAny = new Vector2(0.25f, 0.75f);

        [HideIf("GetEnum1", HideIfEnum.Case2)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnum = new Vector2(0.25f, 0.75f);

        [HideIf("GetEnum2", HideIfEnumFlag.Flag0)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnumFlag;

        [HideIf("GetEnum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 hideIfEnumFlagMulti;
    }

    public enum HideIfEnum
    {
        Case0,
        Case1,
        Case2
    }

    [Flags]
    public enum HideIfEnumFlag
    {
        Flag0 = 1,
        Flag1 = 2,
        Flag2 = 4,
        Flag3 = 8
    }
}
