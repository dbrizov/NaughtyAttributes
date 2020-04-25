using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ButtonTest : MonoBehaviour
	{
		public int myInt;

		[Button(enabledMode: ButtonAttribute.EnableMode.Always)]
		private void IncrementMyInt()
		{
			myInt++;
		}

		[Button("Decrement My Int", ButtonAttribute.EnableMode.Editor)]
		private void DecrementMyInt()
		{
			myInt--;
		}

		[Button(enabledMode: ButtonAttribute.EnableMode.Playmode)]
		private void LogMyInt()
		{
			Debug.Log(myInt);
		}
	}
}
