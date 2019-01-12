using UnityEngine;
using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
    public static class EditorDrawUtility
    {
        public static void DrawHeader(string header)
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(header, EditorStyles.boldLabel);
        }

        public static bool DrawHeader(SerializedProperty property)
        {
            HeaderAttribute headerAttr = PropertyUtility.GetAttribute<HeaderAttribute>(property);
            if (headerAttr != null)
            {
                DrawHeader(headerAttr.header);
                return true;
            }

            return false;
        }

        public static void DrawHelpBox(string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = true)
        {
            EditorGUILayout.HelpBox(message, type);

            if (logToConsole)
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

        public static void DrawPropertyField(SerializedProperty property, bool includeChildren = true)
        {
            EditorGUILayout.PropertyField(property, includeChildren);
        }

        public static bool DrawLayoutField(object value, string label)
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
    }
}
