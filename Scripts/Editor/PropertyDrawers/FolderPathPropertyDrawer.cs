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

            bool browseClicked = false;
            FolderPathAttribute folderPathAttribute = null;

            if (property.propertyType == SerializedPropertyType.String)
            {
                folderPathAttribute = PropertyUtility.GetAttribute<FolderPathAttribute>(property);

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
            }
            else
            {
                string message = string.Format(TypeWarningMessage, property.name);
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();

            // OpenFolderPanel clears the editor's internal property stack while open. Running it from
            // inside OnGUI corrupts Unity's outer PropertyDrawer.OnGUISafe pop, so defer to delayCall.
            if (browseClicked)
            {
                SerializedObject serializedObject = property.serializedObject;
                string propertyPath = property.propertyPath;
                FolderPathAttribute capturedAttribute = folderPathAttribute;
                string currentPath = property.stringValue;

                EditorApplication.delayCall += () =>
                {
                    string defaultPath = string.IsNullOrEmpty(currentPath) ? capturedAttribute.DefaultPath : currentPath;
                    string selectedPath = EditorUtility.OpenFolderPanel(capturedAttribute.Title, defaultPath, "");

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
    }
}
