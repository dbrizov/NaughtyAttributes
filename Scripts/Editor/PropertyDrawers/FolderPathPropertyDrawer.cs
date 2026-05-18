using UnityEngine;
using UnityEditor;
using System.IO;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FolderPathAttribute))]
    public class FolderPathPropertyDrawer : PropertyDrawerBase
    {
        private const string TypeWarningMessage = "{0} must be a string";
        private const string BrowseButtonLabel = "Browse";
        private const float BrowseButtonWidth = 60.0f;

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            return (property.propertyType == SerializedPropertyType.String)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                FolderPathAttribute folderPathAttribute = PropertyUtility.GetAttribute<FolderPathAttribute>(property);

                Rect labelRect = new Rect(
                    rect.x,
                    rect.y,
                    EditorGUIUtility.labelWidth,
                    EditorGUIUtility.singleLineHeight);

                Rect textFieldRect = new Rect(
                    rect.x + EditorGUIUtility.labelWidth,
                    rect.y,
                    rect.width - EditorGUIUtility.labelWidth - BrowseButtonWidth - NaughtyEditorGUI.HorizontalSpacing,
                    EditorGUIUtility.singleLineHeight);

                Rect buttonRect = new Rect(
                    rect.x + rect.width - BrowseButtonWidth,
                    rect.y,
                    BrowseButtonWidth,
                    EditorGUIUtility.singleLineHeight);

                EditorGUI.LabelField(labelRect, label);

                EditorGUI.BeginChangeCheck();
                string newValue = EditorGUI.TextField(textFieldRect, property.stringValue);
                if (EditorGUI.EndChangeCheck())
                {
                    property.stringValue = newValue;
                }

                if (GUI.Button(buttonRect, BrowseButtonLabel))
                {
                    string path = property.stringValue;
                    string defaultPath = string.IsNullOrEmpty(path) ? folderPathAttribute.DefaultPath : path;
                    string selectedPath = EditorUtility.OpenFolderPanel(folderPathAttribute.Title, defaultPath, "");

                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        property.stringValue = selectedPath;
                    }
                }
            }
            else
            {
                string message = string.Format(TypeWarningMessage, property.name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }
    }
}
