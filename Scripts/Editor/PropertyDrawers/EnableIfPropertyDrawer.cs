using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(EnableIfAttribute))]
	public class EnableIfPropertyDrawer : PropertyDrawerBase
	{
		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			EnableIfAttribute enableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>(property);
			object target = PropertyUtility.GetTargetObjectWithProperty(property);
			List<bool> conditionValues = GetConditionValues(enableIfAttribute, target);

			return (conditionValues.Count > 0)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			EnableIfAttribute enableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>(property);
			object target = PropertyUtility.GetTargetObjectWithProperty(property);
			List<bool> conditionValues = GetConditionValues(enableIfAttribute, target);

			if (conditionValues.Count > 0)
			{
				bool enabled;
				if (enableIfAttribute.ConditionOperator == EConditionOperator.And)
				{
					enabled = true;
					foreach (var value in conditionValues)
					{
						enabled = enabled && value;
					}
				}
				else
				{
					enabled = false;
					foreach (var value in conditionValues)
					{
						enabled = enabled || value;
					}
				}

				if (enableIfAttribute.Reversed)
				{
					enabled = !enabled;
				}

				GUI.enabled = enabled;
				EditorGUI.PropertyField(position, property, true);
				GUI.enabled = true;
			}
			else
			{
				string message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
				DrawDefaultPropertyAndHelpBox(position, property, message, MessageType.Warning);
			}
		}

		private List<bool> GetConditionValues(EnableIfAttribute enableIfAttribute, object propertyTarget)
		{
			List<bool> conditionValues = new List<bool>();
			foreach (var condition in enableIfAttribute.Conditions)
			{
				FieldInfo conditionField = ReflectionUtility.GetField(propertyTarget, condition);
				if (conditionField != null &&
					conditionField.FieldType == typeof(bool))
				{
					conditionValues.Add((bool)conditionField.GetValue(propertyTarget));
				}

				PropertyInfo conditionProperty = ReflectionUtility.GetProperty(propertyTarget, condition);
				if (conditionProperty != null &&
					conditionProperty.PropertyType == typeof(bool))
				{
					conditionValues.Add((bool)conditionProperty.GetValue(propertyTarget));
				}

				MethodInfo conditionMethod = ReflectionUtility.GetMethod(propertyTarget, condition);
				if (conditionMethod != null &&
					conditionMethod.ReturnType == typeof(bool) &&
					conditionMethod.GetParameters().Length == 0)
				{
					conditionValues.Add((bool)conditionMethod.Invoke(propertyTarget, null));
				}
			}

			return conditionValues;
		}
	}
}
