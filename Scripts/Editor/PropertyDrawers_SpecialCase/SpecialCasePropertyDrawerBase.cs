using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public abstract class SpecialCasePropertyDrawerBase
	{
		public void OnGUI(SerializedProperty property)
		{
			bool visible = PropertyUtility.IsVisible(property);
			if (!visible)
			{
				return;
			}

			bool enabled = PropertyUtility.IsEnabled(property);
			GUI.enabled = enabled;

			GUIContent overrideLabel = new GUIContent(PropertyUtility.GetLabel(property));
			OnGUI_Internal(property, overrideLabel);

			GUI.enabled = true;
		}

		protected abstract void OnGUI_Internal(SerializedProperty property, GUIContent label);
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
