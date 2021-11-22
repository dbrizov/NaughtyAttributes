using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor {
	public abstract class SpecialCasePropertyDrawerBase {
		public void OnGUI(Rect rect, SerializedProperty property, SpecialCaseDrawerAttribute specialCaseAttribute) {
			// Check if visible
			bool visible = PropertyUtility.IsVisible(property);
			if(!visible) {
				return;
			}

			// Validate
			ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
			foreach(var validatorAttribute in validatorAttributes) {
				validatorAttribute.GetValidator().ValidateProperty(property);
			}

			// Check if enabled and draw
			EditorGUI.BeginChangeCheck();
			bool enabled = PropertyUtility.IsEnabled(property);

			using(new EditorGUI.DisabledScope(disabled: !enabled)) {
				OnGUI_Internal(rect, property, PropertyUtility.GetLabel(property), specialCaseAttribute);
			}

			// Call OnValueChanged callbacks
			if(EditorGUI.EndChangeCheck()) {
				PropertyUtility.CallOnValueChangedCallbacks(property);
			}
		}

		public float GetPropertyHeight(SerializedProperty property) {
			return GetPropertyHeight_Internal(property);
		}

		protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label, SpecialCaseDrawerAttribute specialCaseAttribute);
		protected abstract float GetPropertyHeight_Internal(SerializedProperty property);

		public virtual void Enable(SpecialCaseDrawerAttribute specialCaseAttribute, SerializedProperty prop) {
		}
		public virtual void Disable() {
		}
	}

	public static class SpecialCaseDrawerAttributeExtensions {
		private static Dictionary<Type, SpecialCasePropertyDrawerBase> _drawersByAttributeType;

		static SpecialCaseDrawerAttributeExtensions() {
			_drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>();
			_drawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;

		}

		public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr, SerializedProperty prop) {
			SpecialCasePropertyDrawerBase drawer;

			if(attr is HexBrushAttribute) {
				if(MultipleSpecialDrawer.Exists(prop)) {
					return MultipleSpecialDrawer.GetDrawer(prop);
				} else
					return new HexBrushPropertyDrawer(prop, attr);
			}

			if(_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer)) {
				return drawer;
			} else {
				return null;
			}
		}

	}
}
