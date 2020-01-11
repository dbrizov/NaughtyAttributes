// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class PropertyMetaDatabase
	{
		private static Dictionary<Type, PropertyMeta> _metasByAttributeType;

		static PropertyMetaDatabase()
		{
			_metasByAttributeType = new Dictionary<Type, PropertyMeta>();
			_metasByAttributeType[typeof(InfoBoxAttribute)] = new InfoBoxPropertyMeta();
_metasByAttributeType[typeof(OnValueChangedAttribute)] = new OnValueChangedPropertyMeta();

		}

		public static PropertyMeta GetMetaForAttribute(Type attributeType)
		{
			PropertyMeta meta;
			if (_metasByAttributeType.TryGetValue(attributeType, out meta))
			{
				return meta;
			}
			else
			{
				return null;
			}
		}
	}
}

