using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
	public class Dropdowns : MonoBehaviour
	{
		[Dropdown("intValues")]
		public int intValue;

		[Dropdown("StringValues")]
		public string stringValue;

		[Dropdown("GetVectorValues")]
		public Vector3 vectorValue;

#pragma warning disable 414
		private int[] intValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

		private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

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
}
