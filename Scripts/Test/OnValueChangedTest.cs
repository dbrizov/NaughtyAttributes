using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class OnValueChangedTest : MonoBehaviour
	{
		[OnValueChanged("OnValueChangedMethod1")]
		[OnValueChanged("OnValueChangedMethod2")]
		public int int0;

		private void OnValueChangedMethod1(int oldValue, int newValue)
		{
			Debug.LogFormat("First - old: {0}, new: {1}, int0: {2}", oldValue, newValue, int0);
		}

		private void OnValueChangedMethod2(int oldValue, int newValue)
		{
			Debug.LogFormat("Second - old: {0}, new: {1}, int0: {2}", oldValue, newValue, int0);
		}

		public OnValueChangedNest1 nest1;
	}

	[System.Serializable]
	public class OnValueChangedNest1
	{
		[OnValueChanged("OnValueChangedMethod")]
		[AllowNesting]
		public int int1;

		private void OnValueChangedMethod(int oldValue, int newValue)
		{
			Debug.LogFormat("old: {0}, new: {1}, int1: {2}", oldValue, newValue, int1);
		}

		public OnValueChangedNest2 nest2;
	}

	[System.Serializable]
	public class OnValueChangedNest2
	{
		[OnValueChanged("OnValueChangedMethod")]
		[AllowNesting]
		public int int2;

		private void OnValueChangedMethod(int oldValue, int newValue)
		{
			Debug.LogFormat("old: {0}, new: {1}, int2: {2}", oldValue, newValue, int2);
		}
	}
}
