using UnityEngine;
using UnityEditor;
using System.IO;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(FilePathAttribute))]
    public class FilePathPropertyDrawer : PropertyDrawerBase
    {
        private const string TypeWarningMessage = "{0} must be a string";
        private const string FileNotFoundWarningMessage = "{0} does not exist";
        private const string BrowseButtonLabel = "Browse";
        private const float BrowseButtonWidth = 60.0f;

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                return GetPropertyHeight(property) + GetHelpBoxHeight();
            }

            return ShouldShowFileNotFoundWarning(property)
                ? GetPropertyHeight(property) + EditorGUIUtility.standardVerticalSpacing + GetHelpBoxHeight()
                : GetPropertyHeight(property);
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.String)
            {
                FilePathAttribute filePathAttribute = PropertyUtility.GetAttribute<FilePathAttribute>(property);

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
                    string directory = string.IsNullOrEmpty(path) ? filePathAttribute.Directory : Path.GetDirectoryName(path);
                    string selectedPath = EditorUtility.OpenFilePanel(filePathAttribute.Title, directory, filePathAttribute.Filter);

                    if (!string.IsNullOrEmpty(selectedPath))
                    {
                        selectedPath = filePathAttribute.RelativePath
                            ? NaughtyPathUtility.GetProjectRelativePath(selectedPath)
                            : NaughtyPathUtility.NormalizePath(selectedPath);

                        property.stringValue = selectedPath;
                        property.serializedObject.ApplyModifiedProperties();
                        EditorGUIUtility.editingTextField = false;
                        GUIUtility.keyboardControl = 0;
                        GUI.changed = true;
                    }
                }

                if (ShouldShowFileNotFoundWarning(property))
                {
                    Rect helpBoxRect = new Rect(
                        rect.x + NaughtyEditorGUI.GetIndentLength(rect),
                        rect.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                        rect.width - NaughtyEditorGUI.GetIndentLength(rect),
                        GetHelpBoxHeight());

                    string message = string.Format(FileNotFoundWarningMessage, property.stringValue);
                    NaughtyEditorGUI.HelpBox(helpBoxRect, message, MessageType.Warning, property.serializedObject.targetObject);
                }
            }
            else
            {
                string message = string.Format(TypeWarningMessage, property.name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private static bool ShouldShowFileNotFoundWarning(SerializedProperty property)
        {
            FilePathAttribute filePathAttribute = PropertyUtility.GetAttribute<FilePathAttribute>(property);
            if (filePathAttribute == null || !filePathAttribute.ValidateExists || string.IsNullOrEmpty(property.stringValue))
            {
                return false;
            }

            return !File.Exists(NaughtyPathUtility.GetProjectAbsolutePath(property.stringValue));
        }
    }
}
