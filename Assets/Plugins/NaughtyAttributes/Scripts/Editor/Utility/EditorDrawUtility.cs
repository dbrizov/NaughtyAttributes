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

        public static void DrawPropertyField(SerializedProperty property, bool includeChildren = true)
        {
            EditorGUILayout.PropertyField(property, includeChildren);
        }

        public static bool DrawLayoutField(object value, string label)
        {
            GUI.enabled = false;

            bool isDrawn = false;
            Type valueType = value.GetType();

            if (valueType == typeof(int))
            {
                isDrawn = true;
                EditorGUILayout.IntField(label, (int)value);
            }
            else if (valueType == typeof(long))
            {
                isDrawn = true;
                EditorGUILayout.LongField(label, (long)value);
            }
            else if (valueType == typeof(float))
            {
                isDrawn = true;
                EditorGUILayout.FloatField(label, (float)value);
            }
            else if (valueType == typeof(double))
            {
                isDrawn = true;
                EditorGUILayout.DoubleField(label, (double)value);
            }
            else if (valueType == typeof(string))
            {
                isDrawn = true;
                EditorGUILayout.TextField(label, (string)value);
            }
            else if (valueType == typeof(Vector2))
            {
                isDrawn = true;
                EditorGUILayout.Vector2Field(label, (Vector2)value);
            }
            else if (valueType == typeof(Vector3))
            {
                isDrawn = true;
                EditorGUILayout.Vector3Field(label, (Vector3)value);
            }
            else if (valueType == typeof(Vector4))
            {
                isDrawn = true;
                EditorGUILayout.Vector4Field(label, (Vector4)value);
            }
            else if (valueType == typeof(Color))
            {
                isDrawn = true;
                EditorGUILayout.ColorField(label, (Color)value);
            }
            else if (valueType == typeof(Bounds))
            {
                isDrawn = true;
                EditorGUILayout.BoundsField(label, (Bounds)value);
            }
            else if (valueType == typeof(Rect))
            {
                isDrawn = true;
                EditorGUILayout.RectField(label, (Rect)value);
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
