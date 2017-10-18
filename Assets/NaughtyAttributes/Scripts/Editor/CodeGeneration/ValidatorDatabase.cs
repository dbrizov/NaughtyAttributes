// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class ValidatorDatabase
    {
        private static Dictionary<Type, PropertyValidator> validatorsByAttributeType;

        static ValidatorDatabase()
        {
            validatorsByAttributeType = new Dictionary<Type, PropertyValidator>();
            validatorsByAttributeType[typeof(MaxValueAttribute)] = new MaxValuePropertyValidator();
validatorsByAttributeType[typeof(MinValueAttribute)] = new MinValuePropertyValidator();

        }

        public static PropertyValidator GetValidatorForAttribute(Type attributeType)
        {
            PropertyValidator validator;
            if (validatorsByAttributeType.TryGetValue(attributeType, out validator))
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

