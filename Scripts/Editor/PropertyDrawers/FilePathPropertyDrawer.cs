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

            bool browseClicked = false;
            FilePathAttribute filePathAttribute = null;

            if (property.propertyType == SerializedPropertyType.String)
            {
                filePathAttribute = PropertyUtility.GetAttribute<FilePathAttribute>(property);

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

                browseClicked = GUI.Button(buttonRect, BrowseButtonLabel);

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

            // OpenFilePanel clears the editor's internal property stack while open. Running it from
            // inside OnGUI corrupts Unity's outer PropertyDrawer.OnGUISafe pop, so defer to delayCall.
            if (browseClicked)
            {
                SerializedObject serializedObject = property.serializedObject;
                string propertyPath = property.propertyPath;
                FilePathAttribute capturedAttribute = filePathAttribute;
                string currentPath = property.stringValue;

                EditorApplication.delayCall += () =>
                {
                    string directory = string.IsNullOrEmpty(currentPath) ? capturedAttribute.Directory : Path.GetDirectoryName(currentPath);
                    string selectedPath = EditorUtility.OpenFilePanel(capturedAttribute.Title, directory, capturedAttribute.Filter);

                    if (string.IsNullOrEmpty(selectedPath))
                    {
                        return;
                    }

                    selectedPath = capturedAttribute.RelativePath
                        ? NaughtyPathUtility.GetProjectRelativePath(selectedPath)
                        : NaughtyPathUtility.NormalizePath(selectedPath);

                    if (serializedObject.targetObject == null)
                    {
                        return;
                    }

                    serializedObject.Update();
                    SerializedProperty deferredProperty = serializedObject.FindProperty(propertyPath);
                    if (deferredProperty != null)
                    {
                        deferredProperty.stringValue = selectedPath;
                        serializedObject.ApplyModifiedProperties();
                    }

                    EditorGUIUtility.editingTextField = false;
                };
            }
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
