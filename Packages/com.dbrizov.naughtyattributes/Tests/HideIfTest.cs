using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class HideIfTest : MonoBehaviour
	{
		public bool hide1;
		public bool hide2;

		[HideIf(EConditionOperator.And, "hide1", "hide2")]
		[ReorderableList]
		public int[] hideIfAll;

		[HideIf(EConditionOperator.Or, "hide1", "hide2")]
		[ReorderableList]
		public int[] hideIfAny;

		public HideIfNest1 nest1;
	}

	[System.Serializable]
	public class HideIfNest1
	{
		public bool hide1;
		public bool hide2;
		public bool Hide1 { get { return hide1; } }
		public bool Hide2 { get { return hide2; } }

		[HideIf(EConditionOperator.And, "Hide1", "Hide2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int hideIfAll;

		[HideIf(EConditionOperator.Or, "Hide1", "Hide2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int hideIfAny;

		public HideIfNest2 nest2;
	}

	[System.Serializable]
	public class HideIfNest2
	{
		public bool hide1;
		public bool hide2;
		public bool GetHide1() { return hide1; }
		public bool GetHide2() { return hide2; }

		[HideIf(EConditionOperator.And, "GetHide1", "GetHide2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 hideIfAll = new Vector2(0.25f, 0.75f);

		[HideIf(EConditionOperator.Or, "GetHide1", "GetHide2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 hideIfAny = new Vector2(0.25f, 0.75f);
	}
}
