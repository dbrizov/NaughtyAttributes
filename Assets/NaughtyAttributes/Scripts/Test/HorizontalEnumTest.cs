using System;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HorizontalEnumTest : MonoBehaviour
    {
        public enum NormalEnum { Test1, Test2, Test3 }

        #region NormalEnum

        [Space(20)]
        [HorizontalEnum]
        public NormalEnum choose;

        [ShowIf(nameof(choose), NormalEnum.Test1)]
        public bool test1Bool;

        [ShowIf(nameof(choose), NormalEnum.Test1)]
        public int test1Int;

        [ShowIf(nameof(choose), NormalEnum.Test1)]
        public string test1String;


        [ShowIf(nameof(choose), NormalEnum.Test2)]
        public int test2Int;

        [ShowIf(nameof(choose), NormalEnum.Test2)]
        public bool test2Bool;

        [ShowIf(nameof(choose), NormalEnum.Test2)]
        public string test2String;


        [ShowIf(nameof(choose), NormalEnum.Test3)]
        public string test3String;

        [ShowIf(nameof(choose), NormalEnum.Test3)]
        public bool test3Bool;

        [ShowIf(nameof(choose), NormalEnum.Test3)]
        public int test3Int;


        #endregion


        #region FlagsEnum

        [Flags]
        public enum FlagsEnum { None = 0, Test1 = 1 << 0, Test2 = 1 << 1, Test3 = 1 << 2 }

        [Space(20)]
        [HorizontalEnum]
        public FlagsEnum chooseFlagsEnum;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test1)]
        public bool test1BoolFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test1)]
        public int test1IntFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test1)]
        public string test1StringFlags;


        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test2)]
        public int test2IntFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test2)]
        public bool test2BoolFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test2)]
        public string test2StringFlags;


        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test3)]
        public string test3StringFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test3)]
        public bool test3BoolFlags;

        [ShowIf(nameof(chooseFlagsEnum), FlagsEnum.Test3)]
        public int test3IntFlags;
        #endregion
    }
}