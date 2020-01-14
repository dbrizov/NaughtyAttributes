using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[HorizontalLine(color: EColor.Orange)]
		[Header("Orange")]
		[MinMaxSlider(0, 1)]
		public Vector2 minMaxSlider;
	}

	[System.Serializable]
	public class MyClass
	{
		[HorizontalLine(color: EColor.Red)]
		[Header("Red")]
		public MyStruct myStruct;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[HorizontalLine]
		public MyClass myClass;

		[HorizontalLine(color: EColor.Black)]
		[Header("Black")]
		public int dummy;
	}
}
