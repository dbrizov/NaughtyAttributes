// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public static class PropertyDrawerDatabase
	{
		private static Dictionary<Type, PropertyDrawer> _drawersByAttributeType;

		static PropertyDrawerDatabase()
		{
			_drawersByAttributeType = new Dictionary<Type, PropertyDrawer>();
			_drawersByAttributeType[typeof(DisableIfAttribute)] = new DisableIfPropertyDrawer();
_drawersByAttributeType[typeof(DropdownAttribute)] = new DropdownPropertyDrawer();
_drawersByAttributeType[typeof(EnableIfAttribute)] = new EnableIfPropertyDrawer();
_drawersByAttributeType[typeof(InputAxisAttribute)] = new InputAxisPropertyDrawer();
_drawersByAttributeType[typeof(LabelAttribute)] = new LabelPropertyDrawer();
_drawersByAttributeType[typeof(MinMaxSliderAttribute)] = new MinMaxSliderPropertyDrawer();
_drawersByAttributeType[typeof(ProgressBarAttribute)] = new ProgressBarPropertyDrawer();
_drawersByAttributeType[typeof(ReadOnlyAttribute)] = new ReadOnlyPropertyDrawer();
_drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer();
_drawersByAttributeType[typeof(ResizableTextAreaAttribute)] = new ResizableTextAreaPropertyDrawer();
_drawersByAttributeType[typeof(SceneAttribute)] = new ScenePropertyDrawer();
_drawersByAttributeType[typeof(ShowAssetPreviewAttribute)] = new ShowAssetPreviewPropertyDrawer();
_drawersByAttributeType[typeof(SliderAttribute)] = new SliderPropertyDrawer();
_drawersByAttributeType[typeof(TagAttribute)] = new TagPropertyDrawer();

		}

		public static PropertyDrawer GetDrawerForAttribute(Type attributeType)
		{
			PropertyDrawer drawer;
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

