using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ButtonTest : MonoBehaviour
	{
		[Button]
		public void MethodOne()
		{
			Debug.Log("MethodOne()");
		}

		[Button("Button Text")]
		private void MethodTwo()
		{
			Debug.Log("MethodTwo()");
		}
	}
}
