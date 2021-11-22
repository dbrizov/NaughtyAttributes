using LoneTower.EditorUtilis;
using LoneTower.HexSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace LoneTower.EditorUtilis {

	public class SerializedHexBrush : HexBrushMain {

		SerializedObject prop;
		string propName;

		public SerializedHexBrush(string propName, SerializedObject prop, List<HexVector> vv, BrushSettings data) : base(vv, data) {
			this.prop = prop;
			this.propName = propName;
		}
		protected override void EndStroke(HexVector v) {
			base.EndStroke(v);
			HexArraySerializer.Serialize(propName, prop, selection);
		}
	}
}
public static class HexArraySerializer {

	public static void Serialize(string propName, SerializedObject prop, List<HexVector> hex) {
		if(EditorApplication.isPlayingOrWillChangePlaymode)
			return;

		prop.Update();
		SerializedProperty array = prop.FindProperty(propName);
		array.arraySize = hex.Count;
		prop.ApplyModifiedProperties();
		Undo.RecordObjects(prop.targetObjects, "stroke");
		for(int i = 0; i < hex.Count; i++) {
			SerializedProperty x = array.GetArrayElementAtIndex(i).FindPropertyRelative("x");
			SerializedProperty y = array.GetArrayElementAtIndex(i).FindPropertyRelative("y");
			x.intValue = hex[i].x;
			y.intValue = hex[i].y;
		}
		prop.ApplyModifiedProperties();
		EditorUtility.SetDirty(prop.targetObject);
	}
	public static void Serialize(SerializedProperty prop, List<HexVector> hex) {
		if(EditorApplication.isPlayingOrWillChangePlaymode)
			return;

		prop.arraySize = hex.Count;
		for(int i = 0; i < hex.Count; i++) {
			SerializedProperty x = prop.GetArrayElementAtIndex(i).FindPropertyRelative("x");
			SerializedProperty y = prop.GetArrayElementAtIndex(i).FindPropertyRelative("y");
			x.intValue = hex[i].x;
			y.intValue = hex[i].y;
		}

		prop.serializedObject.ApplyModifiedProperties();
		EditorUtility.SetDirty(prop.serializedObject.targetObject);

	}

}
public static class HexBrushGUIDrawer {
	static int size = 30;
	public static void Draw(string name, HexBrushLogic brush) {
		EditorGUILayout.BeginHorizontal();

		bool s = GUILayout.Toggle(brush.on, EditorGUIUtility.IconContent("Grid.PaintTool@2x"), "Button", GUILayout.Width(size), GUILayout.Height(size));
		if(s != brush.on) {
			if(s)
				brush.Enable();
			else
				brush.Disable();
		}


		if(GUILayout.Button(EditorGUIUtility.IconContent("P4_DeletedLocal@2x"), GUILayout.Height(size), GUILayout.Width(size))) {
			brush.ClearSelection();
		}

		EditorGUILayout.LabelField(name + ": " + brush.selection.Count);

		EditorGUILayout.EndHorizontal();

	}
}
