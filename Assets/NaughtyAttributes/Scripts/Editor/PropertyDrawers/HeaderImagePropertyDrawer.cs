using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(HeaderImageAttribute))]
	public class HeaderImagePropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
				Texture2D previewTexture = GetAssetPreview(property);
				if (previewTexture != null)
				{
          // HACK: On my macOS machine, I need to scale the Screen.Width by 1/2
          // to make it work. It may be because Unity supports Retina displays.
          // However, I don't know how to assess that programmatically. For the
          // moment, I'll #ifdef it based on macOS.
          var previewSize = RescaleSize(GetAssetPreviewSize(property),
                                        Screen.dpi > 200 // Is this a Retina display?
                                          ? Screen.width / 2
                                          : Screen.width
                                        );
          // Debug.Log($"Screen width is {Screen.width} preview size {previewSize}");

					return GetPropertyHeight(property) + previewSize.y;
				}
				else
				{
					// return GetPropertyHeight(property);
          return GetPropertyHeight(property) + GetHelpBoxHeight();
				}
		}

    private Vector2 RescaleSize(Vector2 previewSize, float maxWidth) {

      if (previewSize.x > maxWidth) {
        float scale = maxWidth / previewSize.x;
        previewSize.y *= scale;
        previewSize.x = maxWidth;
      }
      return previewSize;
    }

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);
      Texture2D previewTexture = GetAssetPreview(property);
      HeaderImageAttribute headerImageAttribute
        = PropertyUtility.GetAttribute<HeaderImageAttribute>(property);
      if (previewTexture != null)
      {

        float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
        var previewSize = GetAssetPreviewSize(property);

        float width = rect.width - indentLength;
        previewSize = RescaleSize(previewSize, width);
        float alignmentLength = 0f;
        if (previewSize.x < width) {
          switch (headerImageAttribute.Alignment) {
            case EAlignment.Center:
              alignmentLength = (width - previewSize.x) / 2f;
              break;
            case EAlignment.Right:
              alignmentLength = (width - previewSize.x);
              break;
            case EAlignment.Left:
            default:
              alignmentLength = 0f;
              break;
          }
        }
				Rect propertyRect = new Rect()
				{
					x = rect.x,
					y = rect.y + previewSize.y,
					width = rect.width,
					height = EditorGUIUtility.singleLineHeight
				};

				EditorGUI.PropertyField(propertyRect, property, label);

					Rect previewRect = new Rect()
					{
						x = rect.x + indentLength + alignmentLength,
						y = rect.y,
						width = rect.width,
						height = previewSize.y
					};

					GUI.Label(previewRect, previewTexture);
				}
			else
			{
				string message = property.name + " no header image for path: " + headerImageAttribute.Path;
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private Texture2D GetAssetPreview(SerializedProperty property)
		{
      HeaderImageAttribute headerImageAttribute = PropertyUtility.GetAttribute<HeaderImageAttribute>(property);
			Texture2D previewTexture = AssetDatabase.LoadAssetAtPath<Texture2D>(headerImageAttribute.Path);
      return previewTexture;
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
				HeaderImageAttribute headerImageAttribute
          = PropertyUtility.GetAttribute<HeaderImageAttribute>(property);
				int width = headerImageAttribute.Width.HasValue
          ? Mathf.Clamp(headerImageAttribute.Width.Value, 0, previewTexture.width)
          : previewTexture.width;

				int height = headerImageAttribute.Height.HasValue
          ? Mathf.Clamp(headerImageAttribute.Height.Value, 0, previewTexture.height)
          : previewTexture.height;
				return new Vector2(width, height);
			}
		}
	}
}
