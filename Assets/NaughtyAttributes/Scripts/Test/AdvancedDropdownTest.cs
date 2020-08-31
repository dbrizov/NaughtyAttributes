using UnityEngine;
using System.Collections.Generic;
using System;

namespace NaughtyAttributes.Test
{
	public class AdvancedDropdownTest : MonoBehaviour
	{
		[AdvancedDropdown("intValues", MinHeight = 150, DrawOnSameLine = true)]
		[OnValueChanged(nameof(OnValueChanged))]
		public int intValue;

		private void OnValueChanged()
		{
			intValue2 = intValue;
		}

		public int intValue2;

#pragma warning disable 414
		private int[] intValues = new int[] { 1, 2, 3, 4 };
#pragma warning restore 414

		public AdvancedDropdownNest1 nest1;
	}

	[System.Serializable]
	public class AdvancedDropdownNest1
	{
		[AdvancedDropdown("StringValues")]
		public string stringValue;

		private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

		public AdvancedDropdownNest2 nest2;
	}

	[System.Serializable]
	public class AdvancedDropdownNest2
	{
		[AdvancedDropdown("GetVectorValues", ListTitle = "the best vectors")]
		[OnValueChanged(nameof(OnValueChanged))]
		public Vector3 vectorValue;

		[AdvancedDropdown("GetVectorValues", FlattenTree = true)]
		[OnValueChanged(nameof(OnValueChanged))]
		public Vector3 vectorValue3;

		private void OnValueChanged()
		{
			vectorValue2 = vectorValue;
		}

		private DropdownList<Vector3> GetVectorValues()
		{
			return new DropdownList<Vector3>()
			{
				{ "Right", Vector3.right },
				{ "Up", Vector3.up },
				{ "Forward", Vector3.forward },
				{ "Others/Left", Vector3.left },
				{ "Others/Down", Vector3.down },
				{ "Others/Back", Vector3.back },
			};
		}

		public Vector3 vectorValue2;
	}
}
