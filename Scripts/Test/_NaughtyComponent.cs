using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		[Label("Short Name")]
		public string veryVeryLongName;

		[Label("RGB")]
		public Vector3 vectorXYZ;
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
