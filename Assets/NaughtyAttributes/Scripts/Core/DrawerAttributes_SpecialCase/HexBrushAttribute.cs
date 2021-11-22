using System;
using UnityEngine;

namespace NaughtyAttributes {
	[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
	public class HexBrushAttribute : SpecialCaseDrawerAttribute {
		public enum HexBrush { Additive, Main }
		public HexBrush brushType;

		public HexBrushAttribute(HexBrush type = HexBrush.Main) {
			brushType = type;
		}

	}
}
