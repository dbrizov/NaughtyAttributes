using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[HorizontalLine(color: EColor.Orange)]
		[Header("Orange")]
		[Dropdown("GetValues")]
		public int level3;

		private int[] GetValues()
		{
			return new int[] { 1, 2, 3 };
		}
	}

	[System.Serializable]
	public class MyClass
	{
		[HorizontalLine(color: EColor.Red)]
		[Header("Red")]
		[Dropdown("GetValues")]
		public int level2;

		public MyStruct myStruct;

		private int[] GetValues()
		{
			return new int[] { 1, 2, 3 };
		}
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[HorizontalLine(color: EColor.Black)]
		[Header("Black")]
		[Dropdown("GetValues")]
		public int level1;

		public MyClass myClass;

		private int[] GetValues()
		{
			return new int[] { 1, 2, 3 };
		}
	}
}
