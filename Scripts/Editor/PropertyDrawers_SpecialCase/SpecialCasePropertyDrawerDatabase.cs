using System.Collections.Generic;
using System;

namespace NaughtyAttributes.Editor
{
	public static class SpecialCasePropertyDrawerDatabase
	{
		private static Dictionary<Type, SpecialCasePropertyDrawer> _drawersByAttributeType;

		static SpecialCasePropertyDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, SpecialCasePropertyDrawer>();
			_drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer();
		}

		public static SpecialCasePropertyDrawer GetDrawerForAttribute(Type attributeType)
		{
			SpecialCasePropertyDrawer drawer;
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
