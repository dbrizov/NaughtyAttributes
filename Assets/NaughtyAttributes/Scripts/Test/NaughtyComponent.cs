using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		public int level3;
	}

	[System.Serializable]
	public class MyClass
	{
		public int level2;

		public MyStruct myStruct;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		public int level1;

		[ReadOnly]
		public MyClass myClass;
	}
}
