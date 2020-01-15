using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
	public static class EditorGUIExtensions
	{
		internal static void _PropertyField_Layout(SerializedProperty property, bool includeChildren)
		{
			ISpecialCaseDrawerAttribute specialCaseAttribute = PropertyUtility.GetAttribute<ISpecialCaseDrawerAttribute>(property);
			if (specialCaseAttribute != null)
			{
				specialCaseAttribute.GetDrawer().OnGUI(property);
			}
			else
			{
				EditorGUILayout.PropertyField(property, includeChildren);
			}
		}

		public static float GetIndentLength(Rect sourceRect)
		{
			Rect indentRect = EditorGUI.IndentedRect(sourceRect);
			float indentLength = indentRect.x - sourceRect.x;

			return indentLength;
		}

		/// <summary>
		/// Creates a dropdown
		/// </summary>
		/// <param name="rect">The rect the defines the position and size of the dropdown in the inspector/param>
		/// <param name="serializedObject">The serialized object that is being updated</param>
		/// <param name="target">The target object that contains the dropdown</param>
		/// <param name="dropdownField">The field of the target object that holds the currently selected dropdown value</param>
		/// <param name="label">The label of the dropdown</param>
		/// <param name="selectedValueIndex">The index of the value from the values array</param>
		/// <param name="values">The values of the dropdown</param>
		/// <param name="displayOptions">The display options for the values</param>
		public static void Dropdown(
			Rect rect, SerializedObject serializedObject, object target, FieldInfo dropdownField,
			string label, int selectedValueIndex, object[] values, string[] displayOptions)
		{
			EditorGUI.BeginChangeCheck();

			int newIndex = EditorGUI.Popup(rect, label, selectedValueIndex, displayOptions);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(serializedObject.targetObject, "Dropdown");

				// TODO: Problem with structs, because they are value type.
				// The solution is to make boxing/unboxing but unfortunately I don't know the compile time type of the target object
				dropdownField.SetValue(target, values[newIndex]);
			}
		}

		public static void Button(UnityEngine.Object target, MethodInfo methodInfo)
		{
			if (methodInfo.GetParameters().Length == 0)
			{
				ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
				string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

				if (GUILayout.Button(buttonText))
				{
					methodInfo.Invoke(target, null);
				}
			}
			else
			{
				string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
				HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
		}

		public static void HorizontalLine(Rect rect, float height, Color color)
		{
			rect.height = height;
			EditorGUI.DrawRect(rect, color);
		}

		public static void HelpBox(Rect rect, string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = true)
		{
			EditorGUI.HelpBox(rect, message, type);

			if (logToConsole)
			{
				DebugLogMessage(message, type, context);
			}
		}

		public static void HelpBox_Layout(string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = true)
		{
			EditorGUILayout.HelpBox(message, type);

			if (logToConsole)
			{
				DebugLogMessage(message, type, context);
			}
		}

		public static bool LayoutField(object value, string label)
		{
			GUI.enabled = false;

			bool isDrawn = true;
			Type valueType = value.GetType();

			if (valueType == typeof(bool))
			{
				EditorGUILayout.Toggle(label, (bool)value);
			}
			else if (valueType == typeof(int))
			{
				EditorGUILayout.IntField(label, (int)value);
			}
			else if (valueType == typeof(long))
			{
				EditorGUILayout.LongField(label, (long)value);
			}
			else if (valueType == typeof(float))
			{
				EditorGUILayout.FloatField(label, (float)value);
			}
			else if (valueType == typeof(double))
			{
				EditorGUILayout.DoubleField(label, (double)value);
			}
			else if (valueType == typeof(string))
			{
				EditorGUILayout.TextField(label, (string)value);
			}
			else if (valueType == typeof(Vector2))
			{
				EditorGUILayout.Vector2Field(label, (Vector2)value);
			}
			else if (valueType == typeof(Vector3))
			{
				EditorGUILayout.Vector3Field(label, (Vector3)value);
			}
			else if (valueType == typeof(Vector4))
			{
				EditorGUILayout.Vector4Field(label, (Vector4)value);
			}
			else if (valueType == typeof(Color))
			{
				EditorGUILayout.ColorField(label, (Color)value);
			}
			else if (valueType == typeof(Bounds))
			{
				EditorGUILayout.BoundsField(label, (Bounds)value);
			}
			else if (valueType == typeof(Rect))
			{
				EditorGUILayout.RectField(label, (Rect)value);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(valueType))
			{
				EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, valueType, true);
			}
			else
			{
				isDrawn = false;
			}

			GUI.enabled = true;

			return isDrawn;
		}

		private static void DebugLogMessage(string message, MessageType type, UnityEngine.Object context)
		{
			switch (type)
			{
				case MessageType.None:
				case MessageType.Info:
					Debug.Log(message, context);
					break;
				case MessageType.Warning:
					Debug.LogWarning(message, context);
					break;
				case MessageType.Error:
					Debug.LogError(message, context);
					break;
			}
		}
	}
}
