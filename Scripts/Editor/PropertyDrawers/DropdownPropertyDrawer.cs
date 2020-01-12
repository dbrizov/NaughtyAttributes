using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Reflection;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DropdownAttribute))]
	public class DropdownPropertyDrawer : NaughtyPropertyDrawer
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
			object values = GetValues(property, dropdownAttribute.ValuesName);
			FieldInfo fieldInfo = ReflectionUtility.GetField(property.serializedObject.targetObject, property.name);

			return AreValuesValid(values, fieldInfo)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(position, label, property);

			DropdownAttribute dropdownAttribute = (DropdownAttribute)attribute;
			UnityEngine.Object target = property.serializedObject.targetObject;

			object valuesObject = GetValues(property, dropdownAttribute.ValuesName);
			FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);

			if (AreValuesValid(valuesObject, fieldInfo))
			{
				if (valuesObject is IList && fieldInfo.FieldType == GetElementType(valuesObject))
				{
					// Selected value
					object selectedValue = fieldInfo.GetValue(target);

					// Values and display options
					IList valuesList = (IList)valuesObject;
					object[] values = new object[valuesList.Count];
					string[] displayOptions = new string[valuesList.Count];

					for (int i = 0; i < values.Length; i++)
					{
						object value = valuesList[i];
						values[i] = value;
						displayOptions[i] = value.ToString();
					}

					// Selected value index
					int selectedValueIndex = Array.IndexOf(values, selectedValue);
					if (selectedValueIndex < 0)
					{
						selectedValueIndex = 0;
					}

					EditorGUIExtensions.Dropdown(position, target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);
				}
				else if (valuesObject is IDropdownList)
				{
					// Current value
					object selectedValue = fieldInfo.GetValue(target);

					// Current value index, values and display options
					IDropdownList dropdown = (IDropdownList)valuesObject;
					IEnumerator<KeyValuePair<string, object>> dropdownEnumerator = dropdown.GetEnumerator();

					int index = -1;
					int selectedValueIndex = -1;
					List<object> values = new List<object>();
					List<string> displayOptions = new List<string>();

					while (dropdownEnumerator.MoveNext())
					{
						index++;

						KeyValuePair<string, object> current = dropdownEnumerator.Current;
						if (current.Value.Equals(selectedValue))
						{
							selectedValueIndex = index;
						}

						values.Add(current.Value);
						displayOptions.Add(current.Key);
					}

					if (selectedValueIndex < 0)
					{
						selectedValueIndex = 0;
					}

					EditorGUIExtensions.Dropdown(
						position, target, fieldInfo, property.displayName, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
				}
			}
			else
			{
				string message = string.Format("Invalid values with name '{0}' provided to '{1}'. Either the values name is incorrect or the types of the target field and the values field/property/method don't match",
					dropdownAttribute.ValuesName, dropdownAttribute.GetType().Name);

				DrawDefaultPropertyAndHelpBox(position, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private object GetValues(SerializedProperty property, string valuesName)
		{
			UnityEngine.Object target = property.serializedObject.targetObject;

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

		private bool AreValuesValid(object values, FieldInfo targetFieldInfo)
		{
			if (values == null || targetFieldInfo == null)
			{
				return false;
			}

			if ((values is IList && targetFieldInfo.FieldType == GetElementType(values)) ||
				(values is IDropdownList))
			{
				return true;
			}

			return false;
		}

		private Type GetElementType(object values)
		{
			Type valuesType = values.GetType();
			if (valuesType.IsGenericType)
			{
				return valuesType.GetGenericArguments()[0];
			}
			else
			{
				return valuesType.GetElementType();
			}
		}
	}
}
