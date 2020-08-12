using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
	public class _NaughtyScriptableObject : ScriptableObject
	{
		public int myInt;

		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 mySlider;

		[Button]
		private void IncrementMyInt()
		{
			myInt++;
		}

		[Button("Decrement My Int")]
		private void DecrementMyInt()
		{
			myInt--;
		}
	}
}
