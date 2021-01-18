using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class EnableIfTest : MonoBehaviour
	{
		public bool enable1;
		public bool enable2;

		[EnableIf(EConditionOperator.And, "enable1", "enable2")]
		[ReorderableList]
		public int[] enableIfAll;

		[EnableIf(EConditionOperator.Or, "enable1", "enable2")]
		[ReorderableList]
		public int[] enableIfAny;

		public EnableIfNest1 nest1;
	}

	[System.Serializable]
	public class EnableIfNest1
	{
		public bool enable1;
		public bool enable2;
		public bool Enable1 { get { return enable1; } }
		public bool Enable2 { get { return enable2; } }

		[EnableIf(EConditionOperator.And, "Enable1", "Enable2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int enableIfAll;

		[EnableIf(EConditionOperator.Or, "Enable1", "Enable2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int enableIfAny;

		public EnableIfNest2 nest2;
	}

	[System.Serializable]
	public class EnableIfNest2
	{
		public bool enable1;
		public bool enable2;
		public bool GetEnable1() { return enable1; }
		public bool GetEnable2() { return enable2; }

		[EnableIf(EConditionOperator.And, "GetEnable1", "GetEnable2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 enableIfAll = new Vector2(0.25f, 0.75f);

		[EnableIf(EConditionOperator.Or, "GetEnable1", "GetEnable2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 enableIfAny = new Vector2(0.25f, 0.75f);
	}
}
