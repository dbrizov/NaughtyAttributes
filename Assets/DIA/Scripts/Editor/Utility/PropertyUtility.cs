using UnityEditor;
using System;
using System.Reflection;

public static class PropertyUtility
{
    public static T[] GetAttributes<T>(SerializedProperty property) where T : DiaAttribute
    {
        Type targetType = GetTargetObject(property).GetType();
        FieldInfo fieldInfo = targetType.GetField(property.name);

        return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
    }

    public static UnityEngine.Object GetTargetObject(SerializedProperty property)
    {
        return property.serializedObject.targetObject;
    }
}