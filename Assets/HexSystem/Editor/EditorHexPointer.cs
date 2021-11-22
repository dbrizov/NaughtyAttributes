using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LoneTower.EditorUtilis {

	public class EditorHexPointer {
		public static EditorHexPointer Instance {
			get {
				if(instance == null)
					instance = new EditorHexPointer();
				return instance;
			}
		}
		public static EditorHexPointer instance;

		public Action<HexVector> OnPressed, OnDrag, OnRelease;
		protected HexVector startPos;

		public void Enable() {
			EditorSceneInput.Instance.inputInterception = true;
		}
		public void Disable() {
			EditorSceneInput.Instance.inputInterception = false;
		}

		private void Click() {
			startPos = EditorSceneInput.Instance.MouseHexPosition;
			OnPressed?.Invoke(startPos);
		}

		protected virtual void Pressing() {
			HexVector v = EditorSceneInput.Instance.MouseHexPosition;
			if(v == startPos)
				return;
			OnDrag?.Invoke(v);
		}

		private void Release() {
			HexVector v = EditorSceneInput.Instance.MouseHexPosition;
			OnRelease?.Invoke(v);
		}

		public EditorHexPointer() {
			EditorSceneInput.Instance.MouseDown += Click;
			EditorSceneInput.Instance.MousePressing += Pressing;
			EditorSceneInput.Instance.MouseUp += Release;
		}

		~EditorHexPointer() {
			EditorSceneInput.Instance.MouseDown -= Click;
			EditorSceneInput.Instance.MousePressing -= Pressing;
			EditorSceneInput.Instance.MouseUp -= Release;
		}

	}

	public class EditorHexPointerContinuous : EditorHexPointer {
		protected override void Pressing() {
			HexVector v = EditorSceneInput.Instance.MouseHexPosition;
			OnDrag?.Invoke(v);
		}
	}

}