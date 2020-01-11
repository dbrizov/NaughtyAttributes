// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class PropertyValidatorDatabase
	{
		private static Dictionary<Type, PropertyValidator> _validatorsByAttributeType;

		static PropertyValidatorDatabase()
		{
			_validatorsByAttributeType = new Dictionary<Type, PropertyValidator>();
			_validatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
_validatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();
_validatorsByAttributeType[typeof(RequiredAttribute)] = new RequiredPropertyValidator();
_validatorsByAttributeType[typeof(ValidateInputAttribute)] = new ValidateInputPropertyValidator();

		}

		public static PropertyValidator GetValidatorForAttribute(Type attributeType)
		{
			PropertyValidator validator;
			if (_validatorsByAttributeType.TryGetValue(attributeType, out validator))
			{
				return validator;
			}
			else
			{
				return null;
			}
		}
	}
}

