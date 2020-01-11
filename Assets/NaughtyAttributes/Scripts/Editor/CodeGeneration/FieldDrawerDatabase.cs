// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class FieldDrawerDatabase
	{
		private static Dictionary<Type, FieldDrawer> _drawersByAttributeType;

		static FieldDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, FieldDrawer>();
			_drawersByAttributeType[typeof(ShowNonSerializedFieldAttribute)] = new ShowNonSerializedFieldFieldDrawer();

		}

		public static FieldDrawer GetDrawerForAttribute(Type attributeType)
		{
			FieldDrawer drawer;
			if (_drawersByAttributeType.TryGetValue(attributeType, out drawer))
			{
				return drawer;
			}
			else
			{
				return null;
			}
		}
	}
}

