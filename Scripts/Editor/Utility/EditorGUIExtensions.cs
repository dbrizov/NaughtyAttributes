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
				SpecialCasePropertyDrawer drawer = SpecialCasePropertyDrawerDatabase.GetDrawerForAttribute(specialCaseAttribute.GetType());
				drawer.OnGUI(property);
			}
			else
			{
				EditorGUILayout.PropertyField(property, includeChildren);
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

		public static void HorizontalLine()
		{
			EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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
