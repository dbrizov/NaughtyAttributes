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
                Texture2D previewTexture = GetAssetPreview(property);
                if (previewTexture != null)
                {
                    return GetPropertyHeight(property) + GetAssetPreviewSize(property).y;
                }
                else
                {
                    return GetPropertyHeight(property);
                }
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

                Texture2D previewTexture = GetAssetPreview(property);
                if (previewTexture != null)
                {
                    Rect previewRect = new Rect()
                    {
                        x = rect.x + NaughtyEditorGUI.GetIndentLength(rect),
                        y = rect.y + EditorGUIUtility.singleLineHeight,
                        width = rect.width,
                        height = GetAssetPreviewSize(property).y
                    };

                    GUI.Label(previewRect, previewTexture);
                }
            }
            else
            {
                string message = property.name + " doesn't have an asset preview";
                DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
            }

            EditorGUI.EndProperty();
        }

        private Texture2D GetAssetPreview(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    return previewTexture;
                }

                return null;
            }

            return null;
        }

        private Vector2 GetAssetPreviewSize(SerializedProperty property)
        {
            Texture2D previewTexture = GetAssetPreview(property);
            if (previewTexture == null)
            {
                return Vector2.zero;
            }
            else
            {
                int targetWidth = ShowAssetPreviewAttribute.DefaultWidth;
                int targetHeight = ShowAssetPreviewAttribute.DefaultHeight;

                ShowAssetPreviewAttribute showAssetPreviewAttribute = PropertyUtility.GetAttribute<ShowAssetPreviewAttribute>(property);
                if (showAssetPreviewAttribute != null)
                {
                    targetWidth = showAssetPreviewAttribute.Width;
                    targetHeight = showAssetPreviewAttribute.Height;
                }

                int width = Mathf.Clamp(targetWidth, 0, previewTexture.width);
                int height = Mathf.Clamp(targetHeight, 0, previewTexture.height);

                return new Vector2(width, height);
            }
        }
    }
}
