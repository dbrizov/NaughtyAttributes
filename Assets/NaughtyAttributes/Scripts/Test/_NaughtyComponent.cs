using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		[ReorderableList]
		public int[] list;

		public MyClass myClass;
	}

	[System.Serializable]
	public class MyClass
	{
		public string level1;

		public MyStruct myStruct;
	}

	[System.Serializable]
	public struct MyStruct
	{
		[ResizableTextArea]
		public string level2;
	}
}
