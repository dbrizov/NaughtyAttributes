using UnityEditor;
using System;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    public static class PropertyUtility
    {
        public static T GetAttribute<T>(SerializedProperty property) where T : Attribute
        {
            T[] attributes = GetAttributes<T>(property);
            return attributes.Length > 0 ? attributes[0] : null;
        }

        public static T[] GetAttributes<T>(SerializedProperty property) where T : Attribute
        {
            Type targetType = GetTargetObject(property).GetType();
            FieldInfo fieldInfo = targetType.GetField(property.name, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

            return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
        }

        public static UnityEngine.Object GetTargetObject(SerializedProperty property)
        {
            return property.serializedObject.targetObject;
        }
    }
}
