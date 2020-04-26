using UnityEditor;
using System.Reflection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public static class PropertyUtility
	{
		public static T GetAttribute<T>(SerializedProperty property) where T : class
		{
			T[] attributes = GetAttributes<T>(property);
			return (attributes.Length > 0) ? attributes[0] : null;
		}

		public static T[] GetAttributes<T>(SerializedProperty property) where T : class
		{
			FieldInfo fieldInfo = ReflectionUtility.GetField(GetTargetObjectWithProperty(property), property.name);
			if (fieldInfo == null)
			{
				return new T[] { };
			}

			return (T[])fieldInfo.GetCustomAttributes(typeof(T), true);
		}

		public static string GetLabel(SerializedProperty property)
		{
			LabelAttribute labelAttribute = GetAttribute<LabelAttribute>(property);
			return (labelAttribute == null)
				? property.displayName
				: labelAttribute.Label;
		}

		public static void CallOnValueChangedCallbacks(SerializedProperty property)
		{
			OnValueChangedAttribute[] onValueChangedAttributes = GetAttributes<OnValueChangedAttribute>(property);
			if (onValueChangedAttributes.Length == 0)
			{
				return;
			}

			object target = GetTargetObjectWithProperty(property);
			property.serializedObject.ApplyModifiedProperties(); // We must apply modifications so that the new value is updated in the serialized object

			foreach (var onValueChangedAttribute in onValueChangedAttributes)
			{
				MethodInfo callbackMethod = ReflectionUtility.GetMethod(target, onValueChangedAttribute.CallbackName);
				if (callbackMethod != null &&
					callbackMethod.ReturnType == typeof(void) &&
					callbackMethod.GetParameters().Length == 0)
				{
					callbackMethod.Invoke(target, new object[] { });
				}
				else
				{
					string warning = string.Format(
						"{0} can invoke only methods with 'void' return type and 0 parameters",
						onValueChangedAttribute.GetType().Name);

					Debug.LogWarning(warning, property.serializedObject.targetObject);
				}
			}
		}

		public static bool IsEnabled(SerializedProperty property)
		{
			EnableIfAttributeBase enableIfAttribute = GetAttribute<EnableIfAttributeBase>(property);
			if (enableIfAttribute == null)
			{
				return true;
			}

			object target = GetTargetObjectWithProperty(property);

			List<bool> conditionValues = GetConditionValues(target, enableIfAttribute.Conditions);
			if (conditionValues.Count > 0)
			{
				bool enabled = GetConditionsFlag(conditionValues, enableIfAttribute.ConditionOperator, enableIfAttribute.Inverted);
				return enabled;
			}
			else
			{
				string message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
				Debug.LogWarning(message, property.serializedObject.targetObject);

				return false;
			}
		}

		public static bool IsVisible(SerializedProperty property)
		{
			ShowIfAttributeBase showIfAttribute = GetAttribute<ShowIfAttributeBase>(property);
			if (showIfAttribute == null)
			{
				return true;
			}

			object target = GetTargetObjectWithProperty(property);

			List<bool> conditionValues = GetConditionValues(target, showIfAttribute.Conditions);
			if (conditionValues.Count > 0)
			{
				bool enabled = GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator, showIfAttribute.Inverted);
				return enabled;
			}
			else
			{
				string message = showIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
				Debug.LogWarning(message, property.serializedObject.targetObject);

				return false;
			}
		}

		internal static List<bool> GetConditionValues(object target, string[] conditions)
		{
			List<bool> conditionValues = new List<bool>();
			foreach (var condition in conditions)
			{
				FieldInfo conditionField = ReflectionUtility.GetField(target, condition);
				if (conditionField != null &&
					conditionField.FieldType == typeof(bool))
				{
					conditionValues.Add((bool)conditionField.GetValue(target));
				}

				PropertyInfo conditionProperty = ReflectionUtility.GetProperty(target, condition);
				if (conditionProperty != null &&
					conditionProperty.PropertyType == typeof(bool))
				{
					conditionValues.Add((bool)conditionProperty.GetValue(target));
				}

				MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, condition);
				if (conditionMethod != null &&
					conditionMethod.ReturnType == typeof(bool) &&
					conditionMethod.GetParameters().Length == 0)
				{
					conditionValues.Add((bool)conditionMethod.Invoke(target, null));
				}
			}

			return conditionValues;
		}

		internal static bool GetConditionsFlag(List<bool> conditionValues, EConditionOperator conditionOperator, bool invert)
		{
			bool flag;
			if (conditionOperator == EConditionOperator.And)
			{
				flag = true;
				foreach (var value in conditionValues)
				{
					flag = flag && value;
				}
			}
			else
			{
				flag = false;
				foreach (var value in conditionValues)
				{
					flag = flag || value;
				}
			}

			if (invert)
			{
				flag = !flag;
			}

			return flag;
		}

		/// <summary>
		/// Gets the object the property represents.
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public static object GetTargetObjectOfProperty(SerializedProperty property)
		{
			if (property == null)
			{
				return null;
			}

			string path = property.propertyPath.Replace(".Array.data[", "[");
			object obj = property.serializedObject.targetObject;
			string[] elements = path.Split('.');

			foreach (var element in elements)
			{
				if (element.Contains("["))
				{
					string elementName = element.Substring(0, element.IndexOf("["));
					int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = GetValue_Imp(obj, elementName, index);
				}
				else
				{
					obj = GetValue_Imp(obj, element);
				}
			}

			return obj;
		}

		/// <summary>
		/// Gets the object that the property is a member of
		/// </summary>
		/// <param name="property"></param>
		/// <returns></returns>
		public static object GetTargetObjectWithProperty(SerializedProperty property)
		{
			string path = property.propertyPath.Replace(".Array.data[", "[");
			object obj = property.serializedObject.targetObject;
			string[] elements = path.Split('.');

			for (int i = 0; i < elements.Length - 1; i++)
			{
				string element = elements[i];
				if (element.Contains("["))
				{
					string elementName = element.Substring(0, element.IndexOf("["));
					int index = Convert.ToInt32(element.Substring(element.IndexOf("[")).Replace("[", "").Replace("]", ""));
					obj = GetValue_Imp(obj, elementName, index);
				}
				else
				{
					obj = GetValue_Imp(obj, element);
				}
			}

			return obj;
		}

		private static object GetValue_Imp(object source, string name)
		{
			if (source == null)
			{
				return null;
			}

			Type type = source.GetType();

			while (type != null)
			{
				FieldInfo field = type.GetField(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
				if (field != null)
				{
					return field.GetValue(source);
				}

				PropertyInfo property = type.GetProperty(name, BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
				if (property != null)
				{
					return property.GetValue(source, null);
				}

				type = type.BaseType;
			}

			return null;
		}

		private static object GetValue_Imp(object source, string name, int index)
		{
			IEnumerable enumerable = GetValue_Imp(source, name) as IEnumerable;
			if (enumerable == null)
			{
				return null;
			}

			IEnumerator enumerator = enumerable.GetEnumerator();
			for (int i = 0; i <= index; i++)
			{
				if (!enumerator.MoveNext())
				{
					return null;
				}
			}

			return enumerator.Current;
		}
	}
}
