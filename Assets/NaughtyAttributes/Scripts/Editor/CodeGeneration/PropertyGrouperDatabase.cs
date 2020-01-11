// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class PropertyGrouperDatabase
	{
		private static Dictionary<Type, PropertyGrouper> _groupersByAttributeType;

		static PropertyGrouperDatabase()
		{
			_groupersByAttributeType = new Dictionary<Type, PropertyGrouper>();
			_groupersByAttributeType[typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper();

		}

		public static PropertyGrouper GetGrouperForAttribute(Type attributeType)
		{
			PropertyGrouper grouper;
			if (_groupersByAttributeType.TryGetValue(attributeType, out grouper))
			{
				return grouper;
			}
			else
			{
				return null;
			}
		}
	}
}

