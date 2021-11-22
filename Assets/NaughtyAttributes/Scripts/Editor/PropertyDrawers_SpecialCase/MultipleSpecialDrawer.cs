using NaughtyAttributes;
using NaughtyAttributes.Editor;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MultipleSpecialDrawer : SpecialCasePropertyDrawerBase {

	public static Dictionary<Tuple<SerializedObject, string>, MultipleSpecialDrawer> Instances = new Dictionary<Tuple<SerializedObject, string>, MultipleSpecialDrawer>();

	SpecialCaseDrawerAttribute attr;
	SerializedProperty prop;

	public static bool Exists(SerializedProperty prop) {
		return Instances.ContainsKey(new Tuple<SerializedObject, string>(prop.serializedObject, prop.name));
	}
	public static MultipleSpecialDrawer GetDrawer(SerializedProperty prop) {
		return Instances[new Tuple<SerializedObject, string>(prop.serializedObject, prop.name)];
	}


	public MultipleSpecialDrawer(SerializedProperty prop, SpecialCaseDrawerAttribute attr) {
		this.prop = prop;
		this.attr = attr;

		Instances.Add(new Tuple<SerializedObject, string>(prop.serializedObject, prop.name), this);
	}

	protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label, SpecialCaseDrawerAttribute specialCaseAttribute) {

	}


	public override void Disable() {
		base.Disable();
		Instances.Remove(new Tuple<SerializedObject, string>(prop.serializedObject, prop.name));
	}

	protected override float GetPropertyHeight_Internal(SerializedProperty property) {
		return 0;
	}
}
