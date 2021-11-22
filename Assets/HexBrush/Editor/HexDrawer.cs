using LoneTower.EditorUtilis;
using LoneTower.HexSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

[CustomPropertyDrawer(typeof(HexVector))]
public class HexDrawer : PropertyDrawer {
	//bool state = false;
	HexBrushLogic brush = new HexBrushSingle(new List<HexVector>(), new BrushSettings(1, 1));
	SerializedProperty x;
	SerializedProperty y;
	bool changed = false;

	public override void OnGUI(Rect rect, SerializedProperty property, GUIContent label) {


		EditorGUI.BeginProperty(rect, label, property);
		x = property.FindPropertyRelative("x");
		y = property.FindPropertyRelative("y");
		if(changed) {
			changed = false;
			x.intValue = brush.selection[0].x;
			y.intValue = brush.selection[0].y;

			EditorGUI.EndProperty();
			return;
		}
		brush.selection = new List<HexVector>() { new HexVector(x.intValue, y.intValue) };
		Rect input = new Rect(
			rect.x,
			rect.y,
			rect.width, rect.height);

		Vector2Int g = EditorGUI.Vector2IntField(input, label, new Vector2Int(x.intValue, y.intValue));

		Rect button = new Rect(
		rect.width - 30,
		rect.y,
		18, 18);


		bool s = GUI.Toggle(button, brush.on, EditorGUIUtility.IconContent("Grid.PaintTool@2x"), "Button");
		if(s != brush.on) {
			if(s) {
				brush.Enable();
				brush.OnStrokeEnd = Serialize;
				brush.selection = new List<HexVector>() { new HexVector(g.x, g.y) };
			} else {

				brush.Disable();
				brush.OnStrokeEnd = null;
				brush.selection.Clear();
			}
		}




		if(brush.selection.Count == 0) {
			x.intValue = g.x;
			y.intValue = g.y;
		} else {
			x.intValue = brush.selection[0].x;
			y.intValue = brush.selection[0].y;
		}

		EditorGUI.EndProperty();
	}

	public override float GetPropertyHeight(SerializedProperty property, GUIContent label) {
		if(EditorGUIUtility.wideMode)
			return 18;
		else
			return 18 * 2;

	}


	public void Serialize() {
		Debug.Log("called");
		x.intValue = brush.selection[0].x;
		y.intValue = brush.selection[0].y;
		EditorUtility.SetDirty(x.serializedObject.targetObject);
		changed = true;
		brush.Disable();

		brush.OnStrokeEnd = null;


	}
}
