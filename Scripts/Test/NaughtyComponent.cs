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
					{ "Forward", Vector3.forward },
					{ "Back", Vector3.back }
				};
		}
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

		[Button("Button")]
		private void Method()
		{
			Debug.Log("Method");
		}
	}
}
