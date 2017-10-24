using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [FieldDrawer(typeof(ShowNonSerializedFieldAttribute))]
    public class ShowNonSerializedFieldFieldDrawer : FieldDrawer
    {
        public override void DrawField(UnityEngine.Object target, FieldInfo field)
        {
            GUI.enabled = false;

            if (field.FieldType == typeof(int))
            {
                EditorGUILayout.IntField(field.Name, (int)field.GetValue(target));
            }
            else if (field.FieldType == typeof(long))
            {
                EditorGUILayout.LongField(field.Name, (long)field.GetValue(target));
            }
            else if (field.FieldType == typeof(float))
            {
                EditorGUILayout.FloatField(field.Name, (float)field.GetValue(target));
            }
            else if (field.FieldType == typeof(double))
            {
                EditorGUILayout.DoubleField(field.Name, (double)field.GetValue(target));
            }
            else if (field.FieldType == typeof(string))
            {
                EditorGUILayout.TextField(field.Name, (string)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Vector2))
            {
                EditorGUILayout.Vector2Field(field.Name, (Vector2)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Vector3))
            {
                EditorGUILayout.Vector3Field(field.Name, (Vector3)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Vector4))
            {
                EditorGUILayout.Vector4Field(field.Name, (Vector4)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Color))
            {
                EditorGUILayout.ColorField(field.Name, (Color)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Bounds))
            {
                EditorGUILayout.BoundsField(field.Name, (Bounds)field.GetValue(target));
            }
            else if (field.FieldType == typeof(Rect))
            {
                EditorGUILayout.RectField(field.Name, (Rect)field.GetValue(target));
            }
            else
            {
                string warning = string.Format("{0} doesn't support {1} type fields", typeof(ShowNonSerializedFieldAttribute).Name, field.FieldType);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }

            GUI.enabled = true;
        }
    }
}
