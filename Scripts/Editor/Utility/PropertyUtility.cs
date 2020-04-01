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
			object target = GetTargetObjectWithProperty(property);
			FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
			object oldValue = fieldInfo.GetValue(target);
			property.serializedObject.ApplyModifiedProperties(); // We must apply modifications so that the new value is updated in the serialized object
			object newValue = fieldInfo.GetValue(target);

			OnValueChangedAttribute[] onValueChangedAttributes = GetAttributes<OnValueChangedAttribute>(property);
			foreach (var onValueChangedAttribute in onValueChangedAttributes)
			{
				MethodInfo callbackMethod = ReflectionUtility.GetMethod(target, onValueChangedAttribute.CallbackName);
				if (callbackMethod != null &&
					callbackMethod.ReturnType == typeof(void) &&
					callbackMethod.GetParameters().Length == 2)
				{
					ParameterInfo oldValueParam = callbackMethod.GetParameters()[0];
					ParameterInfo newValueParam = callbackMethod.GetParameters()[1];

					if (fieldInfo.FieldType == oldValueParam.ParameterType &&
						fieldInfo.FieldType == newValueParam.ParameterType)
					{
						callbackMethod.Invoke(target, new object[] { oldValue, newValue });
					}
					else
					{
						string warning = string.Format(
							"The field '{0}' and the parameters of callback '{1}' must be of the same type." + Environment.NewLine +
							"Field={2}, Param0={3}, Param1={4}",
							fieldInfo.Name, callbackMethod.Name, fieldInfo.FieldType, oldValueParam.ParameterType, newValueParam.ParameterType);

						Debug.LogWarning(warning, property.serializedObject.targetObject);
					}
				}
				else
				{
					string warning = string.Format(
						"{0} can invoke only methods with 'void' return type and 2 parameters of the same type as the field the attribute was put on",
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

		/// <summary>
        /// Sets the value of the property. Will do a deep copy for value types.
        /// </summary>
        /// <param name="property">Property to be set.</param>
        /// <param name="value">New property value.</param>
        public static void SetValue(SerializedProperty property, object value)
        {
            switch (property.propertyType)
            {
                case SerializedPropertyType.Generic:
                    if (property.isArray)
                    {
                        var arrayValue = (Array)value;
                        property.arraySize = arrayValue.Length;
                        for (int index = 0; index < arrayValue.Length; index++)
                        {
                            var arrayElementProperty = property.FindPropertyRelative($"Array.data[{index}]");
                            SetValue(arrayElementProperty, arrayValue.GetValue(index));
                        }
                    }
                    else
                    {
                        foreach ((string path, object childValue) in MapValueType(value))
                        {
                            var childProperty = property.FindPropertyRelative(path);
                            SetValue(childProperty, childValue);
                        }
                    }
                    break;
                case SerializedPropertyType.Integer:
                    property.intValue = (int) value;
                    break;
                case SerializedPropertyType.Character:
                    property.intValue = (char) value;
                    break;
                case SerializedPropertyType.Enum:
                    property.intValue = Convert.ToInt32(value);
                    break;
                case SerializedPropertyType.LayerMask:
                    property.intValue = (LayerMask) value;
                    break;
                case SerializedPropertyType.Boolean:
                    property.boolValue = (bool) value;
                    break;
                case SerializedPropertyType.Float:
                    property.floatValue = (float) value;
                    break;
                case SerializedPropertyType.String:
                    property.stringValue = (string) value;
                    break;
                case SerializedPropertyType.Color:
                    property.colorValue = (Color) value;
                    break;
                case SerializedPropertyType.ObjectReference:
                    property.objectReferenceValue = (UnityEngine.Object) value;
                    break;
                case SerializedPropertyType.Vector2:
                    property.vector2Value = (Vector2) value;
                    break;
                case SerializedPropertyType.Vector3:
                    property.vector3Value = (Vector3) value;
                    break;
                case SerializedPropertyType.Vector4:
                    property.vector4Value = (Vector4) value;
                    break;
                case SerializedPropertyType.Rect:
                    property.rectValue = (Rect) value;
                    break;
                case SerializedPropertyType.ArraySize:
                    property.arraySize = (int) value;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    // TODO: understand why animation curves values
                    // are not being set inside value types
                    property.animationCurveValue = (AnimationCurve) value;
                    break;
                case SerializedPropertyType.Bounds:
                    property.boundsValue = (Bounds) value;
                    break;
                case SerializedPropertyType.Quaternion:
                    property.quaternionValue = (Quaternion) value;
                    break;
                case SerializedPropertyType.ExposedReference:
                    property.exposedReferenceValue = (UnityEngine.Object) value;
                    break;
                case SerializedPropertyType.Vector2Int:
                    property.vector2IntValue = (Vector2Int) value;
                    break;
                case SerializedPropertyType.Vector3Int:
                    property.vector3IntValue = (Vector3Int) value;
                    break;
                case SerializedPropertyType.RectInt:
                    property.rectIntValue = (RectInt) value;
                    break;
                case SerializedPropertyType.BoundsInt:
                    property.boundsIntValue = (BoundsInt) value;
                    break;
                case SerializedPropertyType.Gradient:
                    //property.gradientValue = value;
                    // TODO: understand why gradient values
                    // are not being set inside value types
                    var t = typeof(SerializedProperty);
                    var p = t.GetProperty("gradientValue", BindingFlags.NonPublic | BindingFlags.Instance);
                    p.SetValue(property, value);
                    break;
                case SerializedPropertyType.ManagedReference:
                    property.managedReferenceValue = value;
                    break;
                default:
                    Debug.LogWarning($"Could not set value for property {property.displayName}.");
                    break;
            }
        }

        private static bool IsKnownOrReferenceType(Type type)
        {
            return !type.IsValueType
                   || type.IsEnum
                   || type == typeof(int)
                   || type == typeof(bool)
                   || type == typeof(float)
                   || type == typeof(char)
                   || type == typeof(LayerMask)
                   || type == typeof(ExposedReference<>)
                   || type == typeof(Color)
                   || type == typeof(Vector2)
                   || type == typeof(Vector3)
                   || type == typeof(Vector4)
                   || type == typeof(Rect)
                   || type == typeof(Bounds)
                   || type == typeof(Quaternion)
                   || type == typeof(Vector2Int)
                   || type == typeof(Vector3Int)
                   || type == typeof(RectInt)
                   || type == typeof(BoundsInt);
        }

        private static IEnumerable<(string, object)> MapValueType(object value, string basePath = "")
        {
            if (value == null)
            {
                yield break;
            }

            Type type = value.GetType();
            if (IsKnownOrReferenceType(type))
            {
                yield return (basePath, value);
                yield break;
            }

            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (FieldInfo fieldInfo in fields)
            {
                if (fieldInfo.IsPublic || fieldInfo.GetCustomAttribute<SerializeField>() != null)
                {
                    string path = string.IsNullOrEmpty(basePath) ? fieldInfo.Name : $"{basePath}.{fieldInfo.Name}";
                    foreach ((string, object) child in MapValueType(fieldInfo.GetValue(value), path))
                    {
                        yield return child;
                    }
                }
            }
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
