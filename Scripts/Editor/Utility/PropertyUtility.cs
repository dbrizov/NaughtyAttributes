using UnityEditor;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
	public static class PropertyUtility
	{
		public static T GetAttribute<T>(SerializedProperty property) where T : class
		{
			T[] attributes = GetAttributes<T>(property);
			return attributes.Length > 0 ? attributes[0] : null;
		}

		public static T[] GetAttributes<T>(SerializedProperty property) where T : class
		{
			FieldInfo fieldInfo = ReflectionUtility.GetField(property.serializedObject.targetObject, property.name);
			return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
		}
	}
}
