using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ReadOnlyTest : MonoBehaviour
	{
		[ReadOnly]
		public int readOnlyInt = 5;

		public ReadOnlyNest1 nest1;
	}

	[System.Serializable]
	public class ReadOnlyNest1
	{
		[ReadOnly]
		public float readOnlyFloat = 3.14f;

		public ReadOnlyNest2 nest2;
	}

	[System.Serializable]
	public struct ReadOnlyNest2
	{
		[ReadOnly]
		public string readOnlyString;
	}
}
