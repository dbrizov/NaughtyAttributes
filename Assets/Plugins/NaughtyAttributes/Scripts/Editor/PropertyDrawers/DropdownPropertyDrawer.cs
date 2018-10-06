using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Collections;
using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(DropdownAttribute))]
    public class DropdownPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            DropdownAttribute dropdownAttribute = PropertyUtility.GetAttribute<DropdownAttribute>(property);
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
            FieldInfo valuesFieldInfo = ReflectionUtility.GetField(target, dropdownAttribute.ValuesFieldName);

            if (valuesFieldInfo == null)
            {
                this.DrawWarningBox(string.Format("{0} cannot find a values field with name \"{1}\"", dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName));
                EditorGUILayout.PropertyField(property, true);
            }
            else if (valuesFieldInfo.GetValue(target) is IList &&
                     fieldInfo.FieldType == this.GetElementType(valuesFieldInfo))
            {
                // Selected value
                object selectedValue = fieldInfo.GetValue(target);

                // Values and display options
                IList valuesList = (IList)valuesFieldInfo.GetValue(target);
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

                // Draw the dropdown
                this.DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values, displayOptions);
            }
            else if (valuesFieldInfo.GetValue(target) is IDropdownList)
            {
                // Current value
                object selectedValue = fieldInfo.GetValue(target);

                // Current value index, values and display options
                IDropdownList dropdown = (IDropdownList)valuesFieldInfo.GetValue(target);
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

                // Draw the dropdown
                this.DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
            }
            else
            {
                this.DrawWarningBox(typeof(DropdownAttribute).Name + " works only when the type of the field is equal to the element type of the array");
                EditorGUILayout.PropertyField(property, true);
            }
        }

        private Type GetElementType(FieldInfo listFieldInfo)
        {
            if (listFieldInfo.FieldType.IsGenericType)
            {
                return listFieldInfo.FieldType.GetGenericArguments()[0];
            }
            else
            {
                return listFieldInfo.FieldType.GetElementType();
            }
        }

        private void DrawDropdown(UnityEngine.Object target, FieldInfo fieldInfo, string label, int selectedValueIndex, object[] values, string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int newIndex = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Dropdown");
                fieldInfo.SetValue(target, values[newIndex]);
            }
        }

        private void DrawWarningBox(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
            Debug.LogWarning(message);
        }
    }
}
