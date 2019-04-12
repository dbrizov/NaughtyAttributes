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
            var list = FindValues(target, dropdownAttribute.ValuesFieldName);

            if (list == null)
            {
                DrawWarningBox(string.Format("{0} cannot find a member with name \"{1}\"", dropdownAttribute.GetType().Name, dropdownAttribute.ValuesFieldName));
                EditorGUILayout.PropertyField(property, true);
            }
            else if (list is IList)
            {
                // Selected value
                object selectedValue = fieldInfo.GetValue(target);

                // Values and display options
                IList valuesList = list as IList;
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
            else if (list is IDropdownList)
            {
                // Current value
                object selectedValue = fieldInfo.GetValue(target);

                // Current value index, values and display options
                IDropdownList dropdown = list as IDropdownList;
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
                DrawDropdown(target, fieldInfo, property.displayName, selectedValueIndex, values.ToArray(), displayOptions.ToArray());
            }
            else
            {
                DrawWarningBox(typeof(DropdownAttribute).Name + " works only when the type of the field is equal to the element type of the array");
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

        private object FindValues(object target, string fieldName)
        {
            var type = target.GetType();
            var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            var memberTypes = MemberTypes.Field | MemberTypes.Property | MemberTypes.Method;
            var members = type.GetMember(fieldName, memberTypes, bindingFlags);

            if (members.Length > 0)
            {
                var member = members[0];
                switch (member.MemberType)
                {
                    case MemberTypes.Field:
                        return ((FieldInfo)member).GetValue(target);
                    case MemberTypes.Property:
                        return ((PropertyInfo)member).GetValue(target);
                    case MemberTypes.Method:
                        return ((MethodInfo)member).Invoke(target, null);
                    default:
                        throw new NotSupportedException(string.Format(
                            "The membertype {0} is unsupported!",
                            member.MemberType
                        ));
                }
            }

            return null;
        }
    }
}
