using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class HexTest : MonoBehaviour
	{
		[Hex]
		public int hexHexAssign = 0xA5A5;
		[Hex]
		public int hexDecimalAssign = 1234;
		[Hex]
		public ushort hexShort = 0x1234;
		[Hex]
		public byte hexByte = 0xCC;
		[Hex]
		public ulong hexULong = 0xFFFFFFFF_12345678;
		[Hex(MinimumDisplayWidth = 8)]
		public int hexMinimumWidth8;
		[Hex(MinimumDisplayWidth = 4)]
		public int hexMinimumWidth4 = 0x123;
		[Hex(MinimumDisplayWidth = 1)]
		public int hexMinimumWidth1 = 0x123;
		[Hex(MinimumDisplayWidth = 0)]
		public int hexMinimumWidth0 = 0x123;
		[Hex, MaxValue(ushort.MaxValue)]
		public int hexMaximumValue = 0x0123FFFF;

		[Hex]
		public float hexCantApply;
		
		public HexNest1 nest1;
	}
	
	[System.Serializable]
	public class HexNest1
	{
		[Hex]
		public int hexNested1 = 0xABCD;

		public HexNest2 nest2;
	}

	[System.Serializable]
	public class HexNest2
	{
		[Hex]
		public uint hexNested2 = 0xFEDCBA98;
	}
}