using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class EnableIfTest : MonoBehaviour
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [EnumFlags] public EnableIfEnumFlag enum2;

        [EnableIf(EConditionOperator.And, "enable1", "enable2")]
        [ReorderableList]
        public int[] enableIfAll;

        [EnableIf(EConditionOperator.Or, "enable1", "enable2")]
        [ReorderableList]
        public int[] enableIfAny;

        [EnableIf("enum1", EnableIfEnum.Case0)]
        [ReorderableList]
        public int[] enableIfEnum;

        [EnableIf("enum2", EnableIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] enableIfEnumFlag;

        [EnableIf("enum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] enableIfEnumFlagMulti;

        public EnableIfNest1 nest1;
    }

    [System.Serializable]
    public class EnableIfNest1
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [EnumFlags] public EnableIfEnumFlag enum2;
        public bool Enable1 { get { return enable1; } }
        public bool Enable2 { get { return enable2; } }
        public EnableIfEnum Enum1 { get { return enum1; } }
        public EnableIfEnumFlag Enum2 { get { return enum2; } }

        [EnableIf(EConditionOperator.And, "Enable1", "Enable2")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfAll;

        [EnableIf(EConditionOperator.Or, "Enable1", "Enable2")]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfAny;

        [EnableIf("Enum1", EnableIfEnum.Case1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnum;

        [EnableIf("Enum2", EnableIfEnumFlag.Flag0)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnumFlag;

        [EnableIf("Enum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [AllowNesting] // Because it's nested we need to explicitly allow nesting
        public int enableIfEnumFlagMulti;

        public EnableIfNest2 nest2;
    }

    [System.Serializable]
    public class EnableIfNest2
    {
        public bool enable1;
        public bool enable2;
        public EnableIfEnum enum1;
        [EnumFlags] public EnableIfEnumFlag enum2;
        public bool GetEnable1() { return enable1; }
        public bool GetEnable2() { return enable2; }
        public EnableIfEnum GetEnum1() { return enum1; }
        public EnableIfEnumFlag GetEnum2() { return enum2; }

        [EnableIf(EConditionOperator.And, "GetEnable1", "GetEnable2")]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfAll = new Vector2(0.25f, 0.75f);

        [EnableIf(EConditionOperator.Or, "GetEnable1", "GetEnable2")]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfAny = new Vector2(0.25f, 0.75f);

        [EnableIf("GetEnum1", EnableIfEnum.Case2)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnum = new Vector2(0.25f, 0.75f);

        [EnableIf("GetEnum2", EnableIfEnumFlag.Flag0)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnumFlag;

        [EnableIf("GetEnum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
        public Vector2 enableIfEnumFlagMulti;
    }

    [System.Serializable]
    public enum EnableIfEnum
    {
        Case0,
        Case1,
        Case2
    }

    [Flags]
    public enum EnableIfEnumFlag
    {
        Flag0 = 1,
        Flag1 = 2,
        Flag2 = 4,
        Flag3 = 8
    }
}
