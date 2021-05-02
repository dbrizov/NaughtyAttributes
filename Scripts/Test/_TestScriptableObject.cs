using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "TestScriptableObject", menuName = "NaughtyAttributes/TestScriptableObject")]
	public class _TestScriptableObject : ScriptableObject
	{
		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider;

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
}