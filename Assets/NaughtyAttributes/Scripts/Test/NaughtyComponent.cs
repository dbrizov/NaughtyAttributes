using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[ResizableTextArea]
		public string level3;
	}

	[System.Serializable]
	public class MyClass
	{
		[ResizableTextArea]
		public string level2;

		public MyStruct myStruct;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[ResizableTextArea]
		public string level1;

		public MyClass myClass;
	}
}
