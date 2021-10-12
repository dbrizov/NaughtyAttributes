using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public abstract class SpecialCasePropertyDrawerBase
	{
		public bool OnGUI(Rect rect, NaughtyProperty naughtyProperty)
		{
			bool changeDetected = false;
			
			// Check if visible
			bool visible = PropertyUtility.IsVisible(naughtyProperty.showIfAttribute, naughtyProperty.serializedProperty);
			if (!visible)
			{
				return false;
			}

			// Validate
			ValidatorAttribute[] validatorAttributes = naughtyProperty.validatorAttributes;
			foreach (var validatorAttribute in validatorAttributes)
			{
				validatorAttribute.GetValidator().ValidateProperty(naughtyProperty.serializedProperty);
			}

			// Check if enabled and draw
			EditorGUI.BeginChangeCheck();
			bool enabled = PropertyUtility.IsEnabled(naughtyProperty.readOnlyAttribute, naughtyProperty.enableIfAttribute, naughtyProperty.serializedProperty);

			using (new EditorGUI.DisabledScope(disabled: !enabled))
			{
				OnGUI_Internal(rect, naughtyProperty.serializedProperty, PropertyUtility.GetLabel(naughtyProperty.labelAttribute, naughtyProperty.serializedProperty));
			}

			// Call OnValueChanged callbacks
			if (EditorGUI.EndChangeCheck())
			{
				changeDetected = true;
				PropertyUtility.CallOnValueChangedCallbacks(naughtyProperty.serializedProperty);
			}

			return changeDetected;
		}

		public float GetPropertyHeight(SerializedProperty property)
		{
			return GetPropertyHeight_Internal(property);
		}

		protected abstract void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label);
		protected abstract float GetPropertyHeight_Internal(SerializedProperty property);
	}

	public static class SpecialCaseDrawerAttributeExtensions
	{
		private static Dictionary<Type, SpecialCasePropertyDrawerBase> _drawersByAttributeType;

		static SpecialCaseDrawerAttributeExtensions()
		{
			_drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawerBase>();
			_drawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;
		}

		public static SpecialCasePropertyDrawerBase GetDrawer(this SpecialCaseDrawerAttribute attr)
		{
			SpecialCasePropertyDrawerBase drawer;
			if (_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
			{
				return drawer;
			}
			else
			{
				return null;
			}
		}
	}
}
