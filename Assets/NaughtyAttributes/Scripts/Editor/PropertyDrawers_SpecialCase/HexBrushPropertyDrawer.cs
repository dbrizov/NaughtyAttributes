using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using NaughtyAttributes;

using NaughtyAttributes.Editor;
using LoneTower.EditorUtilis;
using LoneTower.HexSystem;

public class HexBrushPropertyDrawer : MultipleSpecialDrawer {

	HexBrushLogic brush;
	SerializedProperty prop;

	public HexBrushPropertyDrawer(SerializedProperty prop, SpecialCaseDrawerAttribute attr) : base(prop, attr) {
	}

	protected override float GetPropertyHeight_Internal(SerializedProperty property) {
		return EditorGUI.GetPropertyHeight(property, true);
	}

	protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label, SpecialCaseDrawerAttribute specialCaseAttribute) {
		prop = property;

		if(!(property.isArray && property.type == "HexVector")) {
			string message = typeof(HexBrushAttribute).Name + " can be used only on arrays or lists";
			NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning, context: property.serializedObject.targetObject);
			EditorGUILayout.PropertyField(property, true);
			return;
		}
		HexBrushGUIDrawer.Draw(property.name, brush);
	}

	public override void Disable() {
		base.Disable();
		Serialize();
		brush?.Clear();
		brush = null;
	}

	public override void Enable(SpecialCaseDrawerAttribute specialCaseAttribute, SerializedProperty prop) {
		base.Enable(specialCaseAttribute, prop);
		HexBrushAttribute.HexBrush t = (specialCaseAttribute as HexBrushAttribute).brushType;
		HexVector[] v = new HexVector[prop.arraySize];

		for(int i = 0; i < v.Length; i++) {
			SerializedProperty x = prop.GetArrayElementAtIndex(i).FindPropertyRelative("x");
			SerializedProperty y = prop.GetArrayElementAtIndex(i).FindPropertyRelative("y");
			v[i] = new HexVector(x.intValue, y.intValue);
		}

		switch(t) {
			case HexBrushAttribute.HexBrush.Additive:
				brush = new HexBrushAdditive(new List<HexVector>(v), new BrushSettings(1, 1));
				break;
			case HexBrushAttribute.HexBrush.Main:
				brush = new HexBrushMain(new List<HexVector>(v), new BrushSettings(1, 1));
				break;
		}

	}

	void Serialize() {
		HexArraySerializer.Serialize(prop, brush.selection);
		brush.Clear();
	}

}

