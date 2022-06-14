using System.Globalization;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(HexAttribute))]
	public class HexPropertyDrawer : PropertyDrawerBase
	{
		private const string TypeWarningMessage = "{0} must be an integer type";
		
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			bool validPropertyType = property.propertyType == SerializedPropertyType.Integer;

			return validPropertyType
					   ? GetPropertyHeight(property)
					   : GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			HexAttribute hexAttribute = (HexAttribute)attribute;

			EditorGUI.BeginProperty(rect, label, property);

			switch (property.propertyType)
			{
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label, hexAttribute);
					break;
				default:
					string message = string.Format(TypeWarningMessage, property.name);
					DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
					break;
			}

			EditorGUI.EndProperty();
		}
		
		
		private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, HexAttribute hexAttribute)
		{
			int width = hexAttribute.MinimumDisplayWidth;

			if (width < 0)
			{
				width = hexAttribute.GetDefaultWidthForType(property.type);
			}
			
			string stringValue = width <= 0
									 ? $"0x{property.longValue:X}"
									 : "0x" + property.longValue.ToString($"X{width}");

			stringValue = EditorGUI.TextField(rect, label, stringValue).ToLower();

			if (stringValue.StartsWith("0x"))
			{
				// strip the 0x from the beginning, then parse as hex.
				string no0X = stringValue.Remove(0, 2);
				if (long.TryParse(no0X, NumberStyles.HexNumber, CultureInfo.CurrentCulture, out long resultHex))
				{
					property.longValue = resultHex;
				}
			}
			else if (long.TryParse(stringValue, NumberStyles.Any, CultureInfo.CurrentCulture, out long resultAny))
			{
				// no 0x, parse the number as decimal
				property.longValue = resultAny;
			}
			else if (string.IsNullOrWhiteSpace(stringValue))
			{
				// no value, default to zero.
				property.longValue = 0;
			}
		}
	}
}
