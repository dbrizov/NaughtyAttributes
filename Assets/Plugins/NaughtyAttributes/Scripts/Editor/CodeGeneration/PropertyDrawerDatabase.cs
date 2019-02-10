// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class PropertyDrawerDatabase
    {
        private static Dictionary<Type, PropertyDrawer> drawersByAttributeType;

        static PropertyDrawerDatabase()
        {
            drawersByAttributeType = new Dictionary<Type, PropertyDrawer>();
            drawersByAttributeType[typeof(DisableIfAttribute)] = new DisableIfPropertyDrawer();
drawersByAttributeType[typeof(DropdownAttribute)] = new DropdownPropertyDrawer();
drawersByAttributeType[typeof(EnableIfAttribute)] = new EnableIfPropertyDrawer();
drawersByAttributeType[typeof(LabelAttribute)] = new LabelPropertyDrawer();
drawersByAttributeType[typeof(MinMaxSliderAttribute)] = new MinMaxSliderPropertyDrawer();
drawersByAttributeType[typeof(ProgressBarAttribute)] = new ProgressBarPropertyDrawer();
drawersByAttributeType[typeof(ReadOnlyAttribute)] = new ReadOnlyPropertyDrawer();
drawersByAttributeType[typeof(ReorderableListAttribute)] = new ReorderableListPropertyDrawer();
drawersByAttributeType[typeof(ResizableTextAreaAttribute)] = new ResizableTextAreaPropertyDrawer();
drawersByAttributeType[typeof(ShowAssetPreviewAttribute)] = new ShowAssetPreviewPropertyDrawer();
drawersByAttributeType[typeof(SliderAttribute)] = new SliderPropertyDrawer();
drawersByAttributeType[typeof(TagAttribute)] = new TagPropertyDrawer();

        }

        public static PropertyDrawer GetDrawerForAttribute(Type attributeType)
        {
            PropertyDrawer drawer;
            if (drawersByAttributeType.TryGetValue(attributeType, out drawer))
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
            foreach (var kvp in drawersByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}

