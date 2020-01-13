using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[Dropdown("GetVectorValues")]
		public Vector3 vectorValue;

		private DropdownList<Vector3> GetVectorValues()
		{
			return new DropdownList<Vector3>()
			{
				{ "Right", Vector3.right },
				{ "Up", Vector3.up },
				{ "Forward", Vector3.forward }
			};
		}
	}

	[System.Serializable]
	public class MyClass
	{
		public MyStruct myStruct;

		[Dropdown("IntValues")]
		public int intValues;

		private int[] IntValues
		{
			get
			{
				return new int[] { 0, 1, 2 };
			}
		}
	}

	public class NaughtyComponent : MonoBehaviour
	{
		[Header("Nesting")]
		public MyClass myClass;

		//[Header("No Nesting")]
		//[Dropdown("GetVectorValues")]
		//public Vector3 vectorValue;

		//private DropdownList<Vector3> GetVectorValues()
		//{
		//	return new DropdownList<Vector3>()
		//		{
		//			{ "Right", Vector3.right },
		//			{ "Up", Vector3.up },
		//			{ "Forward", Vector3.forward }
		//		};
		//}

		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider;

		[ReorderableList]
		public List<int> list;

		[Button("Button")]
		private void Method()
		{
			Debug.Log("Method");
		}
	}
}
