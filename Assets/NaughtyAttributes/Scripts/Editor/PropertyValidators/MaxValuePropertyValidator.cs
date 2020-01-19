using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public class MaxValuePropertyValidator : PropertyValidatorBase
	{
		public override void ValidateProperty(SerializedProperty property)
		{
			MaxValueAttribute maxValueAttribute = PropertyUtility.GetAttribute<MaxValueAttribute>(property);

			if (property.propertyType == SerializedPropertyType.Integer)
			{
				if (property.intValue > maxValueAttribute.MaxValue)
				{
					property.intValue = (int)maxValueAttribute.MaxValue;
				}
			}
			else if (property.propertyType == SerializedPropertyType.Float)
			{
				if (property.floatValue > maxValueAttribute.MaxValue)
				{
					property.floatValue = maxValueAttribute.MaxValue;
				}
			}
			else
			{
				string warning = maxValueAttribute.GetType().Name + " can be used only on int or float fields";
				Debug.LogWarning(warning, property.serializedObject.targetObject);
			}
		}
	}
}
