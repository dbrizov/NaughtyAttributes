using UnityEngine;
using System.Reflection;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class ButtonUtility
	{
		public static bool IsEnabled(Object target, MethodInfo method)
		{
			var enabled = true;
			var attributes = method.GetCustomAttributes<EnabledAttribute>();
			foreach (var attribute in attributes)
			{
				if (attribute is EnableIfAttributeBase enableIfAttribute)
				{
					var conditions = PropertyUtility.GetConditionValues(target, enableIfAttribute.Conditions);
					if (conditions.Count > 0)
					{
						enabled &= PropertyUtility.GetConditionsFlag(conditions, enableIfAttribute.ConditionOperator, enableIfAttribute.Inverted);
					}
					else
					{
						string message = enableIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
						Debug.LogWarning(message, target);

					}
					continue;
				}
				enabled &= attribute.Enabled;
			}
			return enabled;
		}

		public static bool IsVisible(Object target, MethodInfo method)
		{
			var visible = true;
			var attributes = method.GetCustomAttributes<ShowAttribute>();

			foreach (var attribute in attributes)
			{
				if (attribute is ShowIfAttributeBase showIfAttribute)
				{
					List<bool> conditionValues = PropertyUtility.GetConditionValues(target, showIfAttribute.Conditions);
					if (conditionValues.Count > 0)
					{
						visible &= PropertyUtility.GetConditionsFlag(conditionValues, showIfAttribute.ConditionOperator, showIfAttribute.Inverted);
					}
					else
					{
						string message = showIfAttribute.GetType().Name + " needs a valid boolean condition field, property or method name to work";
						Debug.LogWarning(message, target);

						visible &= false;
					}
					continue;
				}
				visible &= attribute.Visible;
			}
			return visible;
		}
	}
}
