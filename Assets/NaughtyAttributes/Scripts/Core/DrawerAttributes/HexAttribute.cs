using System;
using UnityEngine;

namespace NaughtyAttributes
{
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class HexAttribute : DrawerAttribute
	{
		public int MinimumDisplayWidth { get; set; }

		public HexAttribute(int minimumDisplayWidth = -1)
		{
			MinimumDisplayWidth = minimumDisplayWidth;
		}

		public int GetDefaultWidthForType(string propertyTypeName)
		{
			return propertyTypeName switch 
			{
				"byte" => 2,
				"sbyte" => 2,
				"char" => 4,
				"int" => 8,
				"uint" => 8,
				"nint" => IntPtr.Size * 2,
				"nuint" => UIntPtr.Size * 2,
				"long" => 16,
				"ulong" => 16,
				"short" => 4,
				"ushort" => 4,
				_ => 0,
			};
		}
	}
}
