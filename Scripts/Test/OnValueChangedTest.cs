using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class OnValueChangedTest : MonoBehaviour
	{
		[OnValueChanged("OnValueChangedMethod1")]
		[OnValueChanged("OnValueChangedMethod2")]
		public int int0;

		private void OnValueChangedMethod1()
		{
			Debug.LogFormat("int0: {0}", int0);
		}

		private void OnValueChangedMethod2()
		{
			Debug.LogFormat("int0: {0}", int0);
		}

		public OnValueChangedNest1 nest1;
	}

	[System.Serializable]
	public class OnValueChangedNest1
	{
		[OnValueChanged("OnValueChangedMethod")]
		[AllowNesting]
		public int int1;

		private void OnValueChangedMethod()
		{
			Debug.LogFormat("int1: {0}", int1);
		}

		public OnValueChangedNest2 nest2;
	}

	[System.Serializable]
	public class OnValueChangedNest2
	{
		[OnValueChanged("OnValueChangedMethod")]
		[AllowNesting]
		public int int2;

		private void OnValueChangedMethod()
		{
			Debug.LogFormat("int2: {0}", int2);
		}
	}
}
