using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LoneTower.EditorUtilis {

	public class HexBrushSingle : HexBrushLogic {
		public HexBrushSingle(List<HexVector> vv, BrushSettings data) : base(vv, data, true) { }
		protected override void EndStroke(HexVector v) {
			selection.Clear();
			selection.Add(v);
			OnStrokeEnd?.Invoke();
		}

		protected override void StartStroke(HexVector obj) {

		}

		protected override void Stroke(HexVector v) {

		}
	}
}