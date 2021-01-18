using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class HorizontalLineTest : MonoBehaviour
	{
		[HorizontalLine(color: EColor.Black)]
		[Header("Black")]
		[HorizontalLine(color: EColor.Blue)]
		[Header("Blue")]
		[HorizontalLine(color: EColor.Gray)]
		[Header("Gray")]
		[HorizontalLine(color: EColor.Green)]
		[Header("Green")]
		[HorizontalLine(color: EColor.Indigo)]
		[Header("Indigo")]
		[HorizontalLine(color: EColor.Orange)]
		[Header("Orange")]
		[HorizontalLine(color: EColor.Pink)]
		[Header("Pink")]
		[HorizontalLine(color: EColor.Red)]
		[Header("Red")]
		[HorizontalLine(color: EColor.Violet)]
		[Header("Violet")]
		[HorizontalLine(color: EColor.White)]
		[Header("White")]
		[HorizontalLine(color: EColor.Yellow)]
		[Header("Yellow")]
		[HorizontalLine(10.0f)]
		[Header("Thick")]
		public int line0;

		public HorizontalLineNest1 nest1;
	}

	[System.Serializable]
	public class HorizontalLineNest1
	{
		[HorizontalLine]
		public int line1;

		public HorizontalLineNest2 nest2;
	}

	[System.Serializable]
	public class HorizontalLineNest2
	{
		[HorizontalLine]
		public int line2;
	}
}
