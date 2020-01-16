using UnityEngine;

namespace NaughtyAttributes.Test
{
	public enum TestEnum
	{
		None = 0,
		B = 1 << 0,
		C = 1 << 1,
		D = 1 << 2,
		E = 1 << 3,
		F = 1 << 4,
		All = ~0
	}

	public class EnumFlagsTest : MonoBehaviour
	{
		[EnumFlags]
		public TestEnum flags0;

		public EnumFlagsNest1 nest1;
	}

	[System.Serializable]
	public class EnumFlagsNest1
	{
		[EnumFlags]
		public TestEnum flags1;

		public EnumFlagsNest2 nest2;
	}

	[System.Serializable]
	public class EnumFlagsNest2
	{
		[EnumFlags]
		public TestEnum flags2;
	}
}
