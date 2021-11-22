using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LoneTower.EditorUtilis {

	public abstract class HexBrushLogic {

		public List<HexVector> selection;
		public Action OnStrokeEnd;
		public Action<List<HexVector>> stroke;
		public bool on = false;
		public BrushSettings data;
		public List<HexVector> allowedArea;

		EditorHexPointer input;

		public HexBrushLogic(List<HexVector> selection, BrushSettings data, bool continuous = false) {
			this.selection = selection;
			SceneView.duringSceneGui += SceneDraw;
			if(continuous)
				input = new EditorHexPointerContinuous();
			else
				input = new EditorHexPointer();
		}

		protected virtual void SceneDraw(SceneView obj) {
			DrawSelection();
			DrawBrush();
			DrawPossible();
		}



		public virtual void Enable() {
			on = true;
			EditorSceneInput.Instance.inputInterception = true;
			EditorSceneInput.Instance.MouseScroll += ChangeBrushSize;

			input.OnPressed += StartStroke;
			input.OnPressed += Stroke;
			input.OnDrag += Stroke;
			input.OnRelease += EndStroke;

		}

		public virtual void Disable() {
			on = false;
			EditorSceneInput.Instance.inputInterception = false;
			EditorSceneInput.Instance.MouseScroll -= ChangeBrushSize;

			input.OnPressed -= StartStroke;
			input.OnPressed -= Stroke;
			input.OnDrag -= Stroke;
			input.OnRelease -= EndStroke;
		}

		//STROKE//////
		protected abstract void StartStroke(HexVector obj);
		protected abstract void Stroke(HexVector v);
		protected virtual void EndStroke(HexVector v) {
			OnStrokeEnd?.Invoke();
		}
		//STROKE//////

		public void ClearSelection() {
			data.subtractive = true;
			stroke?.Invoke(selection);
			selection.Clear();
		}

		public void DrawSelection() {
			Color aaa = Color.blue;
			aaa.a = 0.3f;
			Handles.color = aaa;

			foreach(HexVector v in selection) {
				Handles.DrawSolidDisc(v.Cartesian, Vector3.up, 0.3f);
			}
		}
		public void DrawBrush() {
			if(!on)
				return;
			Color aaa;

			if(selection.Contains(EditorSceneInput.Instance.MouseHexPosition)) {
				aaa = Color.red;
			} else
				aaa = Color.green;

			aaa.a = 0.2f;
			Handles.color = aaa;

			foreach(HexVector a in GetBrush()) {
				Handles.DrawSolidDisc(a.Cartesian, Vector3.up, 0.3f);
			}

		}
		private void DrawPossible() {
			if(allowedArea == null)
				return;

			foreach(HexVector a in allowedArea) {
				Handles.DrawAAPolyLine(10, circle(a.Cartesian, 0.3f, 10));
			}
		}
		protected HexVector[] GetBrush() {
			List<HexVector> v = new List<HexVector>();

			v.Add(EditorSceneInput.Instance.MouseHexPosition);
			for(int r = 1; r < data.size; r++) {
				HexVector m = HexVector.up * r;
				for(int i = 0; i < r * 6; i++) {
					m = m.CircleRotate(1);
					v.Add(EditorSceneInput.Instance.MouseHexPosition + m);
				}
			}
			return v.ToArray();
		}
		protected virtual void ChangeBrushSize(float f) {
			data.size += Math.Sign(f);
			data.size = Mathf.Clamp(data.size, 0, 10);
		}

		public void Clear() {
			Disable();
			SceneView.duringSceneGui -= SceneDraw;
		}

		Vector3[] circle(Vector3 center, float radius, int points) {
			Vector3[] c = new Vector3[points + 1];
			for(int i = 0; i < points; i++) {
				c[i] = center + Quaternion.AngleAxis((360f / points) * i, Vector3.up) * Vector3.forward * radius;
			}
			c[points] = center + Vector3.forward * radius;
			return c;
		}
	}

	public struct BrushSettings {
		public bool subtractive;
		public int size;
		public float strength;

		public BrushSettings(int size, float strength) {
			this.subtractive = false;
			this.size = size;
			this.strength = strength;
		}
	}

}