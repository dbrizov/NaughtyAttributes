using System.Collections.Generic;
using System;

namespace NaughtyAttributes.Editor
{
	public static class SpecialCasePropertyDrawerDatabase
	{
		private static Dictionary<Type, ISpecialCasePropertyDrawer> _drawersByAttributeType;

		static SpecialCasePropertyDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, ISpecialCasePropertyDrawer>();
			_drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer();
		}

		public static ISpecialCasePropertyDrawer GetDrawerForAttribute(Type attributeType)
		{
			ISpecialCasePropertyDrawer drawer;
			if (_drawersByAttributeType.TryGetValue(attributeType, out drawer))
			{
				return drawer;
			}
			else
			{
				return null;
			}
		}

		public static void ClearCache()
		{
			foreach (var kvp in _drawersByAttributeType)
			{
				kvp.Value.ClearCache();
			}
		}
	}
}
