using System;
using System.Collections.Generic;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public interface ISpecialCasePropertyDrawer
	{
		void OnGUI(SerializedProperty property);
	}

	public static class SpecialCaseDrawerAttributeExtensions
	{
		private static Dictionary<Type, ISpecialCasePropertyDrawer> _drawersByAttributeType;

		static SpecialCaseDrawerAttributeExtensions()
		{
			_drawersByAttributeType = new Dictionary<Type, ISpecialCasePropertyDrawer>();
			_drawersByAttributeType[typeof(ReorderableListAttribute)] = ReorderableListPropertyDrawer.Instance;
		}

		public static ISpecialCasePropertyDrawer GetDrawer(this ISpecialCaseDrawerAttribute attr)
		{
			ISpecialCasePropertyDrawer drawer;
			if (_drawersByAttributeType.TryGetValue(attr.GetType(), out drawer))
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
