using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;
using UnityEditor.IMGUI.Controls;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(AdvancedDropdownAttribute))]
	public class AdvancedDropdownPropertyDrawer : PropertyDrawerBase
	{
		private AdvancedDropdownState _dropdownState;

		public class AdvancedDropdownImpl : AdvancedDropdown
		{
			public struct Options
			{
				public IList<string> Values { get; internal set; }
				public string SelectedValue { get; internal set; }
				public Action<int> ItemSelected { get; internal set; }
				public bool FlattenTree { get; internal set; }
				public int MinHeight { get; internal set; }
				public int MaxHeight { get; internal set; }
				public bool MaximizeWidth { get; internal set; }
				public bool KeepStateWhenReopened { get; internal set; }
				public bool DrawOnSameLine { get; internal set; }
				public string Title { get; internal set; }
			}

			private Options _options;

			public AdvancedDropdownImpl(AdvancedDropdownState state, Options options) : base(state)
			{
				_options = options;
			}

			protected override AdvancedDropdownItem BuildRoot()
			{
				var itemNames = _options.Values.Select(o => o.ToString());

				var root = MakeTreeFromPaths(itemNames, _options.Title ?? "Options", _options.FlattenTree ? '\0' : '/');

				return root;
			}

			public AdvancedDropdownItem MakeTreeFromPaths(IEnumerable<string> paths, string rootNodeName = "", char separator = '/')
			{
				int id = 0;
				var rootNode = new AdvancedDropdownItem(rootNodeName);
				foreach (var path in paths.Where(x => !string.IsNullOrEmpty(x.Trim())))
				{
					var currentNode = rootNode;
					var pathItems = path.Split(separator);
					for (int i = 0; i < pathItems.Length; i++)
					{
						var pathItem = pathItems[i];
						var tmp = currentNode.children.FirstOrDefault(x => x.name.Equals(pathItem));

						var currentId = -1;
						Texture2D icon = null;
						if (i == pathItems.Length - 1)
						{
							currentId = id++;
							if (path == _options.SelectedValue.ToString())
								icon = EditorGUIUtility.IconContent("FilterSelectedOnly").image as Texture2D;
						}

						//var type = Type.GetType(pathItem, false, true);
						//if (type != null)
						//{
						//	icon = EditorGUIUtility.ObjectContent(null, type).image as Texture2D;
						//}

						currentNode = tmp ?? AppendItem(currentNode, pathItem, currentId, icon);
					}
				}
				return rootNode;
			}

			private static AdvancedDropdownItem AppendItem(AdvancedDropdownItem currentNode, string item, int id, Texture2D icon = null)
			{
				var ditem = new AdvancedDropdownItem(item)
				{
					id = id,
					icon = icon
				};

				currentNode.AddChild(ditem);
				return ditem;
			}

			protected override void ItemSelected(AdvancedDropdownItem item)
			{
				base.ItemSelected(item);
				_options.ItemSelected(item.id);
			}

			public new void Show(Rect rect)
			{
				base.Show(rect);
				SetHeightForPopup(rect, _options.MinHeight, _options.MaxHeight);
			}

			// based upon https://forum.unity.com/threads/add-maximum-window-size-to-advanceddropdown-control.724229/
			private static void SetHeightForPopup(Rect rect, float minHeight, float maxHeight)
			{
				var window = EditorWindow.focusedWindow;

				if (window == null)
				{
					Debug.LogWarning("EditorWindow.focusedWindow was null.");
					return;
				}

				if (!string.Equals(window.GetType().Namespace, typeof(AdvancedDropdown).Namespace))
				{
					Debug.LogWarning("EditorWindow.focusedWindow " + EditorWindow.focusedWindow.GetType().FullName + " was not in expected namespace.");
					return;
				}

				var position = window.position;
				var hasChanges = false;

				if (minHeight > -1 && position.height < minHeight)
				{
					position.height = minHeight;
					hasChanges = true;
				}

				if (maxHeight > -1 && position.height > maxHeight)
				{
					position.height = maxHeight;
					hasChanges = true;
				}

				if (hasChanges == false)
					return;

				window.minSize = position.size;
				window.maxSize = position.size;
				window.position = position;
				window.ShowAsDropDown(GUIUtility.GUIToScreenRect(rect), position.size);
			}
		}

		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			AdvancedDropdownAttribute dropdownAttribute = (AdvancedDropdownAttribute)attribute;
			object valuesObject = GetValuesObject(property, dropdownAttribute.ValuesName);
			FieldInfo fieldInfo = ReflectionUtility.GetField(PropertyUtility.GetTargetObjectWithProperty(property), property.name);

			float propertyHeight = IsValuesObjectValid(valuesObject, fieldInfo)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();

			return propertyHeight;
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			AdvancedDropdownAttribute dropdownAttribute = (AdvancedDropdownAttribute)attribute;
			object target = PropertyUtility.GetTargetObjectWithProperty(property);

			object valuesObject = GetValuesObject(property, dropdownAttribute.ValuesName);
			FieldInfo dropdownField = ReflectionUtility.GetField(target, property.name);

			if (TryExtractValues(target, valuesObject, dropdownField, out IList<object> values, out IList<string> displayOptions, out int selectedValueIndex, out string selectedValueString))
			{
				ShowAdvancedDropdown(rect, label, new AdvancedDropdownImpl.Options
				{
					Values = displayOptions,
					SelectedValue = selectedValueString,
					FlattenTree = dropdownAttribute.FlattenTree,
					DrawOnSameLine = dropdownAttribute.DrawOnSameLine,
					MinHeight = dropdownAttribute.MinHeight,
					MaxHeight = dropdownAttribute.MaxHeight,
					MaximizeWidth = dropdownAttribute.MaximizeWidth,
					KeepStateWhenReopened = dropdownAttribute.KeepStateWhenReopened,
					Title = dropdownAttribute.ListTitle,
					ItemSelected = selectedIndex =>
					{
						if (selectedIndex == selectedValueIndex)
							return;

						Undo.RecordObject(property.serializedObject.targetObject, "Advanced dropdown");

						dropdownField.SetValue(target, values[selectedIndex]);
						property.serializedObject.ApplyModifiedProperties();

						PropertyUtility.CallOnValueChangedCallbacks(property);
					}
				});

			}
			else
			{
				string message = string.Format("Invalid values with name '{0}' provided to '{1}'. Either the values name is incorrect or the types of the target field and the values field/property/method don't match",
								dropdownAttribute.ValuesName, dropdownAttribute.GetType().Name);

				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private static bool TryExtractValues(object target, object valuesObject, FieldInfo dropdownField, out IList<object> values, out IList<string> displayOptions, out int selectedValueIndex, out string selectedValueString)
		{
			values = null;
			displayOptions = null;
			selectedValueIndex = -1;
			selectedValueString = "";

			if (IsValuesObjectValid(valuesObject, dropdownField) == false)
				return false;

			if (valuesObject is IList)
			{
				ExtractValuesFromList(target, valuesObject, dropdownField, out values, out displayOptions, out selectedValueIndex, out selectedValueString);
				return true;
			}

			if(valuesObject is IDropdownList)
			{
				ExtractValuesFromDropdownList(target, valuesObject, dropdownField, out values, out displayOptions, out selectedValueIndex, out selectedValueString);
				return true;
			}

			return false;
		}


		private static void ExtractValuesFromList(object target, object valuesObject, FieldInfo dropdownField, out IList<object> values, out IList<string> displayOptions, out int selectedValueIndex, out string selectedValueString)
		{
			// Selected value
			object selectedValue = dropdownField.GetValue(target);

			// Values and display options
			IList valuesList = (IList)valuesObject;
			values = new object[valuesList.Count];
			displayOptions = new string[valuesList.Count];
			for (int i = 0; i < valuesList.Count; i++)
			{
				object value = valuesList[i];
				values[i] = value;
				displayOptions[i] = value == null ? "<null>" : value.ToString();
			}

			// Selected value index
			selectedValueIndex = values.IndexOf(selectedValue);

			selectedValueString = selectedValue?.ToString() ?? "<null>";
		}


		private static void ExtractValuesFromDropdownList(object target, object valuesObject, FieldInfo dropdownField, out IList<object> values, out IList<string> displayOptions, out int selectedValueIndex, out string selectedValueString)
		{
			// Current value
			object selectedValue = dropdownField.GetValue(target);

			// Current value index, values and display options
			int index = -1;
			selectedValueIndex = -1;
			selectedValueString = null;

			values = new List<object>();
			displayOptions = new List<string>();
			IDropdownList dropdown = (IDropdownList)valuesObject;

			using (IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator())
			{
				while (dropdownEnumerator.MoveNext())
				{
					index++;

					KeyValuePair<string, object> current = dropdownEnumerator.Current;
					if (current.Value?.Equals(selectedValue) == true)
					{
						selectedValueIndex = index;
						selectedValueString = current.Key;
					}

					values.Add(current.Value);

					if (current.Key == null)
					{
						displayOptions.Add("<null>");
					}
					else if (string.IsNullOrWhiteSpace(current.Key))
					{
						displayOptions.Add("<empty>");
					}
					else
					{
						displayOptions.Add(current.Key);
					}
				}
			}

			if (selectedValueIndex == -1)
				selectedValueString = selectedValue?.ToString() ?? "<null>";
		}

		private void ShowAdvancedDropdown(Rect rect, GUIContent label, AdvancedDropdownImpl.Options options)
		{
			// inspired by https://forum.unity.com/threads/freebie-filedropdown-implementation-using-unityeditor-imgui-controls-advanceddropdown.792477/

			var propertyRect = rect;
			var labelRect = rect;
			labelRect.width -= EditorGUIUtility.labelWidth;
			propertyRect.width -= EditorGUIUtility.labelWidth;
			propertyRect.x = EditorGUIUtility.labelWidth + 20;

			EditorGUI.LabelField(labelRect, label);
			if (EditorGUI.DropdownButton(propertyRect, new GUIContent(options.SelectedValue), FocusType.Keyboard))
			{
				if (options.KeepStateWhenReopened == false || _dropdownState == null)
					_dropdownState = new AdvancedDropdownState();

				var dropdownRect = options.MaximizeWidth ? rect : propertyRect;
				if (options.DrawOnSameLine == true)
					dropdownRect.height = 0;

				var dropdown = new AdvancedDropdownImpl(_dropdownState, options);
				dropdown.Show(dropdownRect);
			}
		}

		private object GetValuesObject(SerializedProperty property, string valuesName)
		{
			object target = PropertyUtility.GetTargetObjectWithProperty(property);

			FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, valuesName);
			if (valuesFieldInfo != null)
			{
				return valuesFieldInfo.GetValue(target);
			}

			PropertyInfo valuesPropertyInfo = ReflectionUtility.GetProperty(target, valuesName);
			if (valuesPropertyInfo != null)
			{
				return valuesPropertyInfo.GetValue(target);
			}

			MethodInfo methodValuesInfo = ReflectionUtility.GetMethod(target, valuesName);
			if (methodValuesInfo != null &&
				methodValuesInfo.ReturnType != typeof(void) &&
				methodValuesInfo.GetParameters().Length == 0)
			{
				return methodValuesInfo.Invoke(target, null);
			}

			return null;
		}

		private static bool IsValuesObjectValid(object values, FieldInfo dropdownField)
		{
			if (values == null || dropdownField == null)
			{
				return false;
			}

			if ((values is IList && dropdownField.FieldType == GetElementType(values)) ||
				(values is IDropdownList))
			{
				return true;
			}

			return false;
		}

		private static Type GetElementType(object values)
		{
			Type valuesType = values.GetType();
			Type elementType = ReflectionUtility.GetListElementType(valuesType);

			return elementType;
		}
	}
}