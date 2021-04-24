using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		[Dropdown("intValues")]
		public int intValue;

#pragma warning disable 414
		private int[] intValues = new int[] { 10, 20, 30 };
#pragma warning restore 414

		[Button]
		private void Log()
		{
			Debug.Log(intValue);
		}
	}

	[System.Serializable]
	public class MyClass
	{
	}

	[System.Serializable]
	public struct MyStruct
	{
	}
}
