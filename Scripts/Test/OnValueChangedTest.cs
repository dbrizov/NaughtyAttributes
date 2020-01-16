using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class OnValueChangedTest : MonoBehaviour
	{
		//[OnValueChanged("OnValueChangedMethod")]
		public int onValueChanged;

		private void OnValueChangedMethod()
		{
			Debug.Log(onValueChanged);
		}
	}
}
