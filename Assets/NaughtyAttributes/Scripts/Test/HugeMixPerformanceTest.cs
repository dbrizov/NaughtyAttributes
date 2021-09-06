using System.Collections;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HugeMixPerformanceTest : MonoBehaviour
    {
        #region AnimatorParamTest
        
        public Animator animator0;

        [AnimatorParam("animator0")]
        public int hash0;

        [AnimatorParam("animator0")]
        public string name0;

        public AnimatorParamNest1 animatorParamNest1;

        [Button("Log 'hash0' and 'name0'")]
        private void TestLog()
        {
            Debug.Log($"hash0 = {hash0}");
            Debug.Log($"name0 = {name0}");
            Debug.Log($"Animator.StringToHash(name0) = {Animator.StringToHash(name0)}");
        }
        
        #endregion

        #region BoxGroupTest
        
        [BoxGroup("Integers")]
        public int int0;
        [BoxGroup("Integers")]
        public int int1;

        [BoxGroup("Floats")]
        public float float0;
        [BoxGroup("Floats")]
        public float float1;

        [BoxGroup("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider0;
        [BoxGroup("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 slider1;

        public string str0;
        public string str1;

        [BoxGroup]
        public Transform trans0;
        [BoxGroup]
        public Transform trans1;
        
        #endregion

        #region ButtonTest
        
        public int myInt;

        [Button(enabledMode: EButtonEnableMode.Always)]
        private void IncrementMyInt()
        {
            myInt++;
        }

        [Button("Decrement My Int", EButtonEnableMode.Editor)]
        private void DecrementMyInt()
        {
            myInt--;
        }

        [Button(enabledMode: EButtonEnableMode.Playmode)]
        private void LogMyInt(string prefix = "MyInt = ")
        {
            Debug.Log(prefix + myInt);
        }

        [Button("StartCoroutine")]
        private IEnumerator IncrementMyIntCoroutine()
        {
            int seconds = 5;
            for (int i = 0; i < seconds; i++)
            {
                myInt++;
                yield return new WaitForSeconds(1.0f);
            }
        }
        
        #endregion

        #region CurveRangeTest
        
        [CurveRange(-1, -1, 1, 1, EColor.Red)] public AnimationCurve curve;

        [CurveRange(EColor.Orange)] public AnimationCurve curve1;

        [CurveRange(0, 0, 10, 10)] public AnimationCurve curve2;

        public CurveRangeNest1 curveRangeNest1;

        [System.Serializable]
        public class CurveRangeNest1
        {
            [CurveRange(0, 0, 1, 1, EColor.Green)] public AnimationCurve curve;

            public CurveRangeNest2 curveRangeNest2;
        }

        [System.Serializable]
        public class CurveRangeNest2
        {
            [CurveRange(0, 0, 5, 5, EColor.Blue)] public AnimationCurve curve;
        }
        
        #endregion

        #region DisableIfTest
        
        public bool disableIf1;
        public bool disableIf2;
        public DisableIfEnum disableIfEnum1;
        [EnumFlags] public DisableIfEnumFlag disableIfEnum2;
        
        [DisableIf(EConditionOperator.And, "disableIf1", "disableIf2")]
        [ReorderableList]
        public int[] disableIfAll;
        
        [DisableIf(EConditionOperator.Or, "disableIf1", "disableIf2")]
        [ReorderableList]
        public int[] disableIfAny;
        
        [DisableIf("disableIfEnum1", DisableIfEnum.Case0)]
        [ReorderableList]
        public int[] disableIfEnum;
        
        [DisableIf("disableIfEnum2", DisableIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] disableIfEnumFlag;
        
        [DisableIf("disableIfEnum2", DisableIfEnumFlag.Flag0 | DisableIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] disableIfEnumFlagMulti;
        
        public DisableIfNest1 disableIfNest1;
        
        #endregion

        #region DropDownTest

        [Dropdown("dropDownIntValues")]
        public int dropDownIntValue;

#pragma warning disable 414
        private int[] dropDownIntValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

        public DropdownNest1 dropdownNest1;

        #endregion

        #region EnableIfTest

        public bool enable1;
        public bool enable2;
        public EnableIfEnum enableIfEnum1;
        [EnumFlags] public EnableIfEnumFlag enableIfEnum2;

        [EnableIf(EConditionOperator.And, "enable1", "enable2")]
        [ReorderableList]
        public int[] enableIfAll;

        [EnableIf(EConditionOperator.Or, "enable1", "enable2")]
        [ReorderableList]
        public int[] enableIfAny;

        [EnableIf("enableIfEnum1", EnableIfEnum.Case0)]
        [ReorderableList]
        public int[] enableIfEnum;

        [EnableIf("enableIfEnum2", EnableIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] enableIfEnumFlag;

        [EnableIf("enableIfEnum2", EnableIfEnumFlag.Flag0 | EnableIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] enableIfEnumFlagMulti;

        public EnableIfNest1 enableIfNest1;

        #endregion

        #region EnumFlagsTest

        [EnumFlags]
        public TestEnum enumFlagsTest0;

        public EnumFlagsNest1 enumFlagsNest1;

        #endregion

        #region ExpandableTest

        [Expandable]
        public ScriptableObject obj0;

        public ExpandableScriptableObjectNest1 expandableScriptableObjectNest1;

        #endregion

        #region FoldoutTest

        [Foldout("Integers")]
        public int foldoutTestInt0;
        [Foldout("Integers")]
        public int foldoutTestInt1;

        [Foldout("Floats")]
        public float foldoutTestFloat0;
        [Foldout("Floats")]
        public float foldoutTestFloat1;

        [Foldout("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 foldoutTestSlider0;
        [Foldout("Sliders")]
        [MinMaxSlider(0, 1)]
        public Vector2 foldoutTestSlider1;

        public string foldoutTestStr0;
        public string foldoutTestStr1;

        [Foldout("Transforms")]
        public Transform foldoutTestTrans0;
        [Foldout("Transforms")]
        public Transform foldoutTestTrans1;

        #endregion

        #region HideIfTest

        public bool hide1;
        public bool hide2;
        public HideIfEnum hideIfEnum1;
        [EnumFlags] public HideIfEnumFlag hideIfEnum2;

        [HideIf(EConditionOperator.And, "hide1", "hide2")]
        [ReorderableList]
        public int[] hideIfAll;

        [HideIf(EConditionOperator.Or, "hide1", "hide2")]
        [ReorderableList]
        public int[] hideIfAny;

        [HideIf("hideIfEnum1", HideIfEnum.Case0)]
        [ReorderableList]
        public int[] hideIfEnum;

        [HideIf("hideIfEnum2", HideIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] hideIfEnumFlag;

        [HideIf("hideIfEnum2", HideIfEnumFlag.Flag0 | HideIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] hideIfEnumFlagMulti;

        public HideIfNest1 hideIfNest1;

        #endregion
        
        #region ShowIfTest
        
        public bool showIf1;
        public bool showIf2;
        public ShowIfEnum showIfEnum1;
        [EnumFlags] public ShowIfEnumFlag showIfEnum2;
        
        [ShowIf(EConditionOperator.And, "showIf1", "showIf2")]
        [ReorderableList]
        public int[] showIfAll;
        
        [ShowIf(EConditionOperator.Or, "showIf1", "showIf2")]
        [ReorderableList]
        public int[] showIfAny;
        
        [ShowIf("showIfEnum1", ShowIfEnum.Case0)]
        [ReorderableList]
        public int[] showIfEnum;
        
        [ShowIf("showIfEnum2", ShowIfEnumFlag.Flag0)]
        [ReorderableList]
        public int[] showIfEnumFlag;
        
        [ShowIf("showIfEnum2", ShowIfEnumFlag.Flag0 | ShowIfEnumFlag.Flag1)]
        [ReorderableList]
        public int[] showIfEnumFlagMulti;
        
        public ShowIfNest1 showIfNest1;
        
        #endregion
    }
}
