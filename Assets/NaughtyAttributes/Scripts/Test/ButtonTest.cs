using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ButtonTest : MonoBehaviour
	{
		public int myInt;

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
