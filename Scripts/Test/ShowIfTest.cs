using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ShowIfTest : MonoBehaviour
	{
		public bool show1;
		public bool show2;

		[ShowIf(EConditionOperator.And, "show1", "show2")]
		[ReorderableList]
		public int[] showIfAll;

		[ShowIf(EConditionOperator.Or, "show1", "show2")]
		[ReorderableList]
		public int[] showIfAny;

		public ShowIfNest1 nest1;
	}

	[System.Serializable]
	public class ShowIfNest1
	{
		public bool show1;
		public bool show2;
		public bool Show1 { get { return show1; } }
		public bool Show2 { get { return show2; } }

		[ShowIf(EConditionOperator.And, "Show1", "Show2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int showIfAll;

		[ShowIf(EConditionOperator.Or, "Show1", "Show2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int showIfAny;

		public ShowIfNest2 nest2;
	}

	[System.Serializable]
	public class ShowIfNest2
	{
		public bool show1;
		public bool show2;
		public bool GetShow1() { return show1; }
		public bool GetShow2() { return show2; }

		[ShowIf(EConditionOperator.And, "GetShow1", "GetShow2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfAll = new Vector2(0.25f, 0.75f);

		[ShowIf(EConditionOperator.Or, "GetShow1", "GetShow2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 showIfAny = new Vector2(0.25f, 0.75f);
	}
}
