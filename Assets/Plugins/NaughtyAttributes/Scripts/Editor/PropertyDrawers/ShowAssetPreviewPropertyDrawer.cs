using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ShowAssetPreviewAttribute))]
    public class ShowAssetPreviewPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorGUILayout.PropertyField(property, true);

            if (property.propertyType == SerializedPropertyType.ObjectReference)
            {
                if (property.objectReferenceValue != null)
                {
                    Texture2D previewTexture = AssetPreview.GetAssetPreview(property.objectReferenceValue);
                    if (previewTexture != null)
                    {
                        ShowAssetPreviewAttribute showAssetPreviewAttribute = PropertyUtility.GetAttributes<ShowAssetPreviewAttribute>(property)[0];
                        int width = Mathf.Clamp(showAssetPreviewAttribute.Width, 0, previewTexture.width);
                        int height = Mathf.Clamp(showAssetPreviewAttribute.Height, 0, previewTexture.height);

                        GUILayout.Label(previewTexture, GUILayout.MaxWidth(width), GUILayout.MaxHeight(height));
                    }
                    else
                    {
                        this.DrawWarningBox(property.name + " doesn't have an asset preview", property);
                    }
                }
            }
            else
            {
                this.DrawWarningBox(property.name + " doesn't have an asset preview", property);
            }
        }

        private void DrawWarningBox(string warningText, SerializedProperty property)
        {
            EditorGUILayout.HelpBox(warningText, MessageType.Warning);
            Debug.LogWarning(warningText, PropertyUtility.GetTargetObject(property));
        }
    }
}
