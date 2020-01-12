using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider;

		[ReorderableList]
		public List<int> list;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[Header("Nesting")]
		public MyStruct myStruct;

		[Header("No Nesting")]
		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider;

		[ReorderableList]
		public List<int> list;
	}
}
