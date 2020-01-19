using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		[ReorderableList]
		public int[] list;

		public MyClass myClass;

#pragma warning disable 414
		[ShowNonSerializedField]
		private int myInt = 10;

		[ShowNonSerializedField]
		private const float PI = 3.14159f;

		[ShowNonSerializedField]
		private static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414

		[ShowNativeProperty]
		private Transform Transform
		{
			get
			{
				return transform;
			}
		}

		[Button]
		private void LogParent()
		{
			Debug.Log(transform.parent);
		}
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
