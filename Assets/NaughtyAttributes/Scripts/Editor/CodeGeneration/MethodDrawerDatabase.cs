// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class MethodDrawerDatabase
	{
		private static Dictionary<Type, MethodDrawer> _drawersByAttributeType;

		static MethodDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, MethodDrawer>();
			_drawersByAttributeType[typeof(ButtonAttribute)] = new ButtonMethodDrawer();

		}

		public static MethodDrawer GetDrawerForAttribute(Type attributeType)
		{
			MethodDrawer drawer;
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

