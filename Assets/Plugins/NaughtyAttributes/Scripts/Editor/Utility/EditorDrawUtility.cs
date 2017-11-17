using UnityEngine;
using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
    public static class EditorDrawUtility
    {
        public static void DrawLayoutField(object value, string label)
        {
            GUI.enabled = false;

            Type valueType = value.GetType();

            if (valueType == typeof(int))
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
            else
            {
                string warning = string.Format("Can't draw {1} type field", valueType.Name);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning);
            }

            GUI.enabled = true;
        }
    }
}
