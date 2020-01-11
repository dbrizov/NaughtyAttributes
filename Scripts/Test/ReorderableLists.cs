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
		//[BoxGroup("Reorderable Lists")]
		//[ReorderableList]
		public int[] intArray;

		//[BoxGroup("Reorderable Lists")]
		//[ReorderableList]
		public List<Vector3> vectorList;

		//[BoxGroup("Reorderable Lists")]
		//[ReorderableList]
		public List<SomeStruct> structList;
	}
}
