using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class DisableIfTest : MonoBehaviour
	{
		public bool disable1;
		public bool disable2;

		[DisableIf(EConditionOperator.And, "disable1", "disable2")]
		[ReorderableList]
		public int[] disableIfAll;

		[DisableIf(EConditionOperator.Or, "disable1", "disable2")]
		[ReorderableList]
		public int[] disableIfAny;

		public DisableIfNest1 nest1;
	}

	[System.Serializable]
	public class DisableIfNest1
	{
		public bool disable1;
		public bool disable2;
		public bool Disable1 { get { return disable1; } }
		public bool Disable2 { get { return disable2; } }

		[DisableIf(EConditionOperator.And, "Disable1", "Disable2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int disableIfAll = 1;

		[DisableIf(EConditionOperator.Or, "Disable1", "Disable2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int disableIfAny = 2;

		public DisableIfNest2 nest2;
	}

	[System.Serializable]
	public class DisableIfNest2
	{
		public bool disable1;
		public bool disable2;
		public bool GetDisable1() { return disable1; }
		public bool GetDisable2() { return disable2; }

		[DisableIf(EConditionOperator.And, "GetDisable1", "GetDisable2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 enableIfAll = new Vector2(0.25f, 0.75f);

		[DisableIf(EConditionOperator.Or, "GetDisable1", "GetDisable2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 enableIfAny = new Vector2(0.25f, 0.75f);
	}
}
