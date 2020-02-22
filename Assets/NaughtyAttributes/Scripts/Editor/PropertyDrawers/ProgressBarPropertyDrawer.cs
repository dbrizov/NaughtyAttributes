using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ProgressBarAttribute))]
	public class ProgressBarPropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			return IsNumber(property)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (!IsNumber(property))
			{
				string message = string.Format("Field {0} is not a number", property.name);
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
				return;
			}

			ProgressBarAttribute progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
			var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
			var valueFormatted = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : string.Format("{0:0.00}", value);
			var fillPercentage = value / progressBarAttribute.MaxValue;
			var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") + valueFormatted + "/" + progressBarAttribute.MaxValue;
			var barColor = progressBarAttribute.Color.GetColor();
			var labelColor = Color.white;

			var indentLength = NaughtyEditorGUI.GetIndentLength(rect);
			Rect barRect = new Rect()
			{
				x = rect.x + indentLength,
				y = rect.y,
				width = rect.width - indentLength,
				height = EditorGUIUtility.singleLineHeight
			};

			DrawBar(barRect, Mathf.Clamp01(fillPercentage), barLabel, barColor, labelColor);
		}

		private void DrawBar(Rect rect, float fillPercent, string label, Color barColor, Color labelColor)
		{
			if (Event.current.type != EventType.Repaint)
			{
				return;
			}

			var fillRect = new Rect(rect.x, rect.y, rect.width * fillPercent, rect.height);

			EditorGUI.DrawRect(rect, new Color(0.13f, 0.13f, 0.13f));
			EditorGUI.DrawRect(fillRect, barColor);

			// set alignment and cache the default
			var align = GUI.skin.label.alignment;
			GUI.skin.label.alignment = TextAnchor.UpperCenter;

			// set the color and cache the default
			var c = GUI.contentColor;
			GUI.contentColor = labelColor;

			// calculate the position
			var labelRect = new Rect(rect.x, rect.y - 2, rect.width, rect.height);

			// draw~
			EditorGUI.DropShadowLabel(labelRect, label);

			// reset color and alignment
			GUI.contentColor = c;
			GUI.skin.label.alignment = align;
		}

		private bool IsNumber(SerializedProperty property)
		{
			bool isNumber = property.propertyType == SerializedPropertyType.Float || property.propertyType == SerializedPropertyType.Integer;
			return isNumber;
		}
	}
}
