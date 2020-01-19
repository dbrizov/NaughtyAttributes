using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class MinMaxValueTest : MonoBehaviour
	{
		[MinValue(0)]
		public int min0;

		[MaxValue(0)]
		public int max0;

		[MinValue(0), MaxValue(1)]
		public float range01;

		public MinMaxValueNest1 nest1;
	}

	[System.Serializable]
	public class MinMaxValueNest1
	{
		[MinValue(0)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int min0;

		[MaxValue(0)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int max0;

		[MinValue(0), MaxValue(1)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public float range01;

		public MinMaxValueNest2 nest2;
	}

	[System.Serializable]
	public class MinMaxValueNest2
	{
		[MinValue(0)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int min0;

		[MaxValue(0)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int max0;

		[MinValue(0), MaxValue(1)]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public float range01;
	}
}
