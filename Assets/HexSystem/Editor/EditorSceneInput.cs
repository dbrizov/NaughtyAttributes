using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace LoneTower.EditorUtilis {
	public class EditorSceneInput : Singleton<EditorSceneInput> {

		public bool inputInterception;
		public bool shiftPressed { get; private set; }
		public event Action MouseDown, MouseUp, MousePressing, ShiftDown, ShiftUp;
		public event Action<float> MouseScroll;
		public event Action<Event> KeyDown;


		bool LMBpressed = false;

		public EditorSceneInput() {
			SceneView.duringSceneGui += SceneFunc;
		}

		void SceneFunc(SceneView sceneView) {
			if(!inputInterception)
				return;

			MouseInput(Event.current);
			ShiftButton(Event.current);
			if(Event.current.type == EventType.KeyDown) {
				KeyDown?.Invoke(Event.current);
			}
		}

		void MouseInput(Event e) {
			HandleUtility.AddDefaultControl(GUIUtility.GetControlID(FocusType.Passive));//disables LBM 

			if(e.type == EventType.MouseDown && e.button == 0) {
				MouseDown?.Invoke();
				LMBpressed = true;

			} else if(e.type == EventType.MouseUp && e.button == 0) {
				MouseUp?.Invoke();
				LMBpressed = false;
			}

			if(LMBpressed) {
				MousePressing?.Invoke();
			}

			if(e.type == EventType.ScrollWheel) {
				MouseScroll?.Invoke(e.delta.y);
				e.Use();
			}
		}

		void ShiftButton(Event e) {
			if(e.type == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftShift) {
				if(!shiftPressed) {

					ShiftDown?.Invoke();
					shiftPressed = true;
				}
			}
			if(e.type == EventType.KeyUp && Event.current.keyCode == KeyCode.LeftShift) {
				if(shiftPressed) {

					ShiftUp?.Invoke();
					shiftPressed = false;
				}
			}
		}

		public HexVector MouseHexPosition {
			get {
				Ray r = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
				float mltp = r.origin.y / r.direction.y;
				Vector3 pos = r.origin - mltp * r.direction;

				return HexVector.GetHex(pos);
			}
		}

	}

}