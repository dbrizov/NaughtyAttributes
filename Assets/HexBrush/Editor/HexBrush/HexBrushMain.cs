using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LoneTower.EditorUtilis {

	public class HexBrushMain : HexBrushLogic {
		public HexBrushMain(List<HexVector> vv, BrushSettings data) : base(vv, data) { }
		protected override void StartStroke(HexVector obj) {
			if(selection.Contains(obj)) {
				data.subtractive = true;
			} else
				data.subtractive = false;
		}

		protected override void Stroke(HexVector v) {
			List<HexVector> modifications = new List<HexVector>();

			if(data.subtractive) {
				foreach(HexVector a in GetBrush()) {
					if(allowedArea != null) {
						if(allowedArea.Contains(a))
							if(selection.Contains(a)) {
								modifications.Add(a);
								selection.Remove(a);
							}
					} else
					if(selection.Contains(a)) {
						modifications.Add(a);
						selection.Remove(a);
					}
				}
			} else {
				foreach(HexVector a in GetBrush()) {
					if(allowedArea != null) {
						if(allowedArea.Contains(a)) {
							if(!selection.Contains(a)) {
								selection.Add(a);
								modifications.Add(a);
							}
						}
					} else if(!selection.Contains(a)) {
						selection.Add(a);
						modifications.Add(a);
					}
				}
			}
			stroke?.Invoke(modifications);
		}


	}
}