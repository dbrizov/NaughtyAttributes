// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class PropertyDrawConditionDatabase
	{
		private static Dictionary<Type, PropertyDrawCondition> _drawConditionsByAttributeType;

		static PropertyDrawConditionDatabase()
		{
			_drawConditionsByAttributeType = new Dictionary<Type, PropertyDrawCondition>();
			_drawConditionsByAttributeType[typeof(HideIfAttribute)] = new HideIfPropertyDrawCondition();
_drawConditionsByAttributeType[typeof(ShowIfAttribute)] = new ShowIfPropertyDrawCondition();

		}

		public static PropertyDrawCondition GetDrawConditionForAttribute(Type attributeType)
		{
			PropertyDrawCondition drawCondition;
			if (_drawConditionsByAttributeType.TryGetValue(attributeType, out drawCondition))
			{
				return drawCondition;
			}
			else
			{
				return null;
			}
		}
	}
}

