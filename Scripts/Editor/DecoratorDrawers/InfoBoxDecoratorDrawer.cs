using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(InfoBoxAttribute))]
	public class InfoBoxDecoratorDrawer : DecoratorDrawer
	{
		public override float GetHeight()
		{
			return GetHelpBoxHeight();
		}

		public override void OnGUI(Rect rect)
		{
			InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)attribute;

			float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
			Rect infoBoxRect = new Rect(
				rect.x + indentLength,
				rect.y,
				rect.width - indentLength,
				GetHelpBoxHeight() - 2.0f);

			DrawInfoBox(infoBoxRect, infoBoxAttribute.Text, infoBoxAttribute.Type);
		}

		private float GetHelpBoxHeight()
		{
			return EditorGUIUtility.singleLineHeight * 3.0f;
		}

		private void DrawInfoBox(Rect rect, string infoText, EInfoBoxType infoBoxType)
		{
			MessageType messageType = MessageType.None;
			switch (infoBoxType)
			{
				case EInfoBoxType.Normal:
					messageType = MessageType.Info;
					break;

				case EInfoBoxType.Warning:
					messageType = MessageType.Warning;
					break;

				case EInfoBoxType.Error:
					messageType = MessageType.Error;
					break;
			}

			NaughtyEditorGUI.HelpBox(rect, infoText, messageType);
		}
	}
}
