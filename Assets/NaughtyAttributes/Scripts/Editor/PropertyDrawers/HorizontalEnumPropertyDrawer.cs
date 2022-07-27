using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(HorizontalEnumAttribute))]
    public class HorizontalEnumPropertyDrawer : PropertyDrawerBase
    {
        protected override void OnGUI_Internal(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            if (property.propertyType == SerializedPropertyType.Enum)
            {
                var hasFlags = fieldInfo.FieldType.GetCustomAttribute<FlagsAttribute>() != null;
                var enumValues = Enum.GetValues(fieldInfo.FieldType).Cast<int>().ToArray();
                var enumNames = property.enumDisplayNames;
                var numberOfButtons = property.enumDisplayNames.Length;

                if (hasFlags) numberOfButtons++;

                var buttonsWidth = position.width / numberOfButtons;
                var nowValue = property.intValue;

                GUILayout.BeginHorizontal();

                for (int i = 0; i < numberOfButtons; i++)
                {
                    var pos = new Rect(
                        position.x + i * buttonsWidth,
                        position.y,
                        buttonsWidth,
                        position.height
                        );


                    bool isSelected, wasSelected;

                    if (hasFlags && i == 0)
                    {
                        var allValue = enumValues.Aggregate(OrOpNumbers);
                        wasSelected = allValue == nowValue;
                        isSelected = Button(pos, wasSelected, "All");
                        if (isSelected != wasSelected)
                        {
                            property.intValue = isSelected ? allValue : 0;
                        }

                        continue;
                    }

                    var fixedIndex = hasFlags ? i - 1 : i;
                    var displayName = enumNames[fixedIndex];
                    var buttonValue = enumValues[fixedIndex];

                    if (hasFlags && buttonValue != 0)
                        wasSelected = (nowValue & buttonValue) == buttonValue;
                    else
                        wasSelected = nowValue == buttonValue;

                    isSelected = Button(pos, wasSelected, displayName);
                    if (wasSelected != isSelected)
                    {
                        if (hasFlags)
                        {
                            if (isSelected && buttonValue != 0) property.intValue |= buttonValue;
                            else if (isSelected) property.intValue = 0;
                            else
                            {
                                var tmpValues = enumValues.ToList();
                                tmpValues.RemoveAt(fixedIndex);

                                property.intValue = tmpValues.Aggregate(OrOpNumbers);
                            }
                        }
                        else if (isSelected)
                        {
                            property.intValue = buttonValue;
                        }
                    }
                }

                GUILayout.EndHorizontal();

            }
            else
            {
                DrawDefaultPropertyAndHelpBox(position, property, $"{nameof(HorizontalEnumAttribute)} Can only be used on enum fields", MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private int OrOpNumbers(int first, int second)
        {
            return first | second;
        }

        private bool Button(Rect pos, bool value, string name)
        {
            var result = EditorGUI.Toggle(pos, value, EditorStyles.miniButton);

            var labelStyle = GUI.skin.label;
            labelStyle.alignment = TextAnchor.MiddleCenter;
            labelStyle.fontStyle = FontStyle.Bold;

            EditorGUI.LabelField(pos, name, labelStyle);

            return result;
        }
    }
}