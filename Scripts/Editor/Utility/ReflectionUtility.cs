using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
	public static class ReflectionUtility
	{
		public static IEnumerable<FieldInfo> GetAllFields(Type type, Func<FieldInfo, bool> predicate)
		{
			List<Type> types = new List<Type>()
			{
				type
			};

			while (types.Last().BaseType != null)
			{
				types.Add(types.Last().BaseType);
			}

			for (int i = types.Count - 1; i >= 0; i--)
			{
				IEnumerable<FieldInfo> fieldInfos = types[i]
					.GetFields(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
					.Where(predicate);

				foreach (var fieldInfo in fieldInfos)
				{
					yield return fieldInfo;
				}
			}
		}

		public static IEnumerable<PropertyInfo> GetAllProperties(Type type, Func<PropertyInfo, bool> predicate)
		{
			List<Type> types = new List<Type>()
			{
				type
			};

			while (types.Last().BaseType != null)
			{
				types.Add(types.Last().BaseType);
			}

			for (int i = types.Count - 1; i >= 0; i--)
			{
				IEnumerable<PropertyInfo> propertyInfos = types[i]
					.GetProperties(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.DeclaredOnly)
					.Where(predicate);

				foreach (var propertyInfo in propertyInfos)
				{
					yield return propertyInfo;
				}
			}
		}

		public static IEnumerable<MethodInfo> GetAllMethods(Type type, Func<MethodInfo, bool> predicate)
		{
			IEnumerable<MethodInfo> methodInfos = type
				.GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
				.Where(predicate);

			return methodInfos;
		}

		public static FieldInfo GetField(Type type, string fieldName)
		{
			return GetAllFields(type, f => f.Name.Equals(fieldName, StringComparison.InvariantCulture)).FirstOrDefault();
		}

		public static PropertyInfo GetProperty(Type type, string propertyName)
		{
			return GetAllProperties(type, p => p.Name.Equals(propertyName, StringComparison.InvariantCulture)).FirstOrDefault();
		}

		public static MethodInfo GetMethod(Type type, string methodName)
		{
			return GetAllMethods(type, m => m.Name.Equals(methodName, StringComparison.InvariantCulture)).FirstOrDefault();
		}
	}
}
