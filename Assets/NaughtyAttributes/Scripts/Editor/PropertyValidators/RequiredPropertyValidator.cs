using UnityEditor;

using static NaughtyAttributes.Editor.PropertyUtility;

namespace NaughtyAttributes.Editor
{
	public class RequiredPropertyValidator : PropertyValidatorBase
	{
		public override void ValidateProperty(SerializedProperty property)
		{
			RequiredAttribute requiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				if (property.objectReferenceValue == null)
				{
					LabelAttribute labelAttribute = GetAttribute<LabelAttribute>(property);
					string labelText = (labelAttribute == null)
						? property.displayName
						: labelAttribute.Label;
					string errorMessage = labelText + " is required";
					if (!string.IsNullOrEmpty(requiredAttribute.Message))
					{
						errorMessage = requiredAttribute.Message;
					}

					NaughtyEditorGUI.HelpBox_Layout(errorMessage, MessageType.Error, context: property.serializedObject.targetObject);
				}
			}
			else
			{
				string warning = requiredAttribute.GetType().Name + " works only on reference types";
				NaughtyEditorGUI.HelpBox_Layout(warning, MessageType.Warning, context: property.serializedObject.targetObject);
			}
		}
	}
}
