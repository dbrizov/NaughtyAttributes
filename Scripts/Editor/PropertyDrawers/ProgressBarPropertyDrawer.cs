using UnityEngine;
using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ProgressBarAttribute))]
    public class ProgressBarPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);

            if (property.propertyType != SerializedPropertyType.Float && property.propertyType != SerializedPropertyType.Integer)
            {
                EditorGUILayout.HelpBox("Field " + property.name + " is not a number", MessageType.Warning);
                return;
            }

            var value = property.propertyType == SerializedPropertyType.Integer ? property.intValue : property.floatValue;
            var valueFormatted = property.propertyType == SerializedPropertyType.Integer ? value.ToString() : String.Format("{0:0.00}", value);

            ProgressBarAttribute progressBarAttribute = PropertyUtility.GetAttribute<ProgressBarAttribute>(property);
            var position = EditorGUILayout.GetControlRect();
            var maxValue = progressBarAttribute.MaxValue;
            float lineHight = EditorGUIUtility.singleLineHeight;
            float padding = EditorGUIUtility.standardVerticalSpacing;
            var barPosition = new Rect(position.position.x, position.position.y, position.size.x, lineHight);

            var fillPercentage = value / maxValue;
            var barLabel = (!string.IsNullOrEmpty(progressBarAttribute.Name) ? "[" + progressBarAttribute.Name + "] " : "") + valueFormatted + "/" + maxValue;

            var color = GetColor(progressBarAttribute.Color);
            var color2 = Color.white;
            DrawBar(barPosition, Mathf.Clamp01(fillPercentage), barLabel, color, color2);
        }

        private void DrawBar(Rect position, float fillPercent, string label, Color barColor, Color labelColor)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            Color savedColor = GUI.color;

            var fillRect = new Rect(position.x, position.y, position.width * fillPercent, position.height);

            EditorGUI.DrawRect(position, new Color(0.13f, 0.13f, 0.13f));
            EditorGUI.DrawRect(fillRect, barColor);

            // set alignment and cache the default
            var align = GUI.skin.label.alignment;
            GUI.skin.label.alignment = TextAnchor.UpperCenter;

            // set the color and cache the default
            var c = GUI.contentColor;
            GUI.contentColor = labelColor;

            // calculate the position
            var labelRect = new Rect(position.x, position.y - 2, position.width, position.height);

            // draw~
            EditorGUI.DropShadowLabel(labelRect, label);

            // reset color and alignment
            GUI.contentColor = c;
            GUI.skin.label.alignment = align;
        }

        private Color GetColor(ProgressBarColor color)
        {
            switch (color)
            {
                case ProgressBarColor.Red:
                    return new Color32(255, 0, 63, 255);
                case ProgressBarColor.Pink:
                    return new Color32(255, 152, 203, 255);
                case ProgressBarColor.Orange:
                    return new Color32(255, 128, 0, 255);
                case ProgressBarColor.Yellow:
                    return new Color32(255, 211, 0, 255);
                case ProgressBarColor.Green:
                    return new Color32(102, 255, 0, 255);
                case ProgressBarColor.Blue:
                    return new Color32(0, 135, 189, 255);
                case ProgressBarColor.Indigo:
                    return new Color32(75, 0, 130, 255);
                case ProgressBarColor.Violet:
                    return new Color32(127, 0, 255, 255);
                default:
                    return Color.white;
            }
        }
    }
}
