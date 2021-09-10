using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public class RequiredPropertyValidator : PropertyValidatorBase
	{
		private RequiredAttribute _cachedRequiredAttribute;
		
		public override void ValidateProperty(SerializedProperty property)
		{
			if (_cachedRequiredAttribute == null)
				_cachedRequiredAttribute = PropertyUtility.GetAttribute<RequiredAttribute>(property);
			
			RequiredAttribute requiredAttribute = _cachedRequiredAttribute;

			if (property.propertyType == SerializedPropertyType.ObjectReference)
			{
				if (property.objectReferenceValue == null)
				{
					string errorMessage = property.name + " is required";
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
