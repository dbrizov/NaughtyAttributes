using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : PropertyDrawerBase
    {
        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue == null)
                {
                    return GetPropertyHeight(property);
                }

                return GetPropertyHeight(property) + GetReservedPreviewSize(property).y;
            }
            else
            {
                return GetPropertyHeight(property) + GetHelpBoxHeight();
            }
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                Rect propertyRect = new Rect()
                {
                    x = rect.x,
                    y = rect.y,
                    width = rect.width,
                    height = EditorGUIUtility.singleLineHeight
                };

                EditorGUI.PropertyField(propertyRect, property, label);

                Object target = property.objectReferenceValue;
                if (target != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(target);
                    if (previewTexture != null)
                    {
                        Rect previewRect = new Rect()
                        {
                            x = rect.x + NaughtyEditorGUI.GetIndentLength(rect),
                            y = rect.y + EditorGUIUtility.singleLineHeight,
                            width = rect.width,
                            height = GetReservedPreviewSize(property).y
                        };

                        GUI.Label(previewRect, previewTexture);
                    }
                    else if (AssetPreview.IsLoadingAssetPreview(target.GetInstanceID()))
                    {
                        EditorWindow focused = EditorWindow.focusedWindow;
                        if (focused != null)
                        {
                            focused.Repaint();
                        }
                    }
                }
            }
            else
            {
                string message = property.name + " doesn't have an asset preview";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private static Vector2 GetReservedPreviewSize(SerializedProperty property)
        {
            int targetWidth = ShowAssetPreviewAttribute.DefaultWidth;
            int targetHeight = ShowAssetPreviewAttribute.DefaultHeight;

            ShowAssetPreviewAttribute showAssetPreviewAttribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
            if (showAssetPreviewAttribute != null)
            {
                targetWidth = showAssetPreviewAttribute.Width;
                targetHeight = showAssetPreviewAttribute.Height;
            }

            return new Vector2(targetWidth, targetHeight);
        }
    }
}
