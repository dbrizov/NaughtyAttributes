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

			Rect infoBoxRect = new Rect(
				rect.x + NaughtyEditorGUI.GetIndentLength(rect),
				rect.y,
				rect.width,
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
