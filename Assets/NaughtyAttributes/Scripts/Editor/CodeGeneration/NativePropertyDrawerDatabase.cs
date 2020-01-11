// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class NativePropertyDrawerDatabase
	{
		private static Dictionary<Type, NativePropertyDrawer> _drawersByAttributeType;

		static NativePropertyDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, NativePropertyDrawer>();
			_drawersByAttributeType[typeof(ShowNativePropertyAttribute)] = new ShowNativePropertyNativePropertyDrawer();

		}

		public static NativePropertyDrawer GetDrawerForAttribute(Type attributeType)
		{
			NativePropertyDrawer drawer;
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

