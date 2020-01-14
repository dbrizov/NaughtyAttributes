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
		public Vector2 level3;
	}

	[System.Serializable]
	public class MyClass
	{
		[HorizontalLine(color: EColor.Red)]
		[Header("Red")]
		[MinMaxSlider(0, 1)]
		public Vector2 level2;

		public MyStruct myStruct;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[HorizontalLine(color: EColor.Black)]
		[Header("Black")]
		[MinMaxSlider(0, 1)]
		public Vector2 level1;

		public MyClass myClass;
	}
}
