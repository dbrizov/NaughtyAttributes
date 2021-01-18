using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class LabelTest : MonoBehaviour
	{
		[Label("Label 0")]
		public int int0;

		public LabelNest1 nest1;
	}

	[System.Serializable]
	public class LabelNest1
	{
		[Label("Label 1")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int int1;

		public LabelNest2 nest2;
	}

	[System.Serializable]
	public class LabelNest2
	{
		[Label("Label 2")]
		[MinMaxSlider(0.0f, 1.0f)] // AllowNesting attribute is not needed, because the field is already marked with a custom naughty property drawer
		public Vector2 vector2;
	}
}
