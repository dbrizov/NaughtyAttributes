using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LoneTower.EditorUtilis {

	public class HexBrushAdditive : HexBrushLogic {
		public HexBrushAdditive(List<HexVector> vv, BrushSettings data) : base(vv, data, true) { }

		protected override void EndStroke(HexVector v) {
			base.EndStroke(v);
		}

		protected override void StartStroke(HexVector obj) {
			data.subtractive = EditorSceneInput.Instance.shiftPressed;
		}

		protected override void Stroke(HexVector v) {
			List<HexVector> modified = new List<HexVector>();

			foreach(HexVector a in GetBrush())
				if(selection.Contains(a)) {
					if(allowedArea != null) {
						if(allowedArea.Contains(a))
							modified.Add(a);
					} else
						modified.Add(a);
				}
			stroke?.Invoke(modified);
		}
	}
}