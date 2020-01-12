using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct SomeStruct
	{
		public int Int;
		public float Float;
		public Vector3 Vector;
	}

	public class ReorderableLists : MonoBehaviour
	{
		[ReorderableList]
		public int[] intArray;

		[ReorderableList]
		public List<Vector3> vectorList;

		[ReorderableList]
		public List<SomeStruct> structList;
	}
}
