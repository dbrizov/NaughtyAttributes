using System;
using System.Collections.Generic;

public static class DrawerUtility
{
    private static Dictionary<Type, PropertyDrawer> drawersByAttributeType;

    static DrawerUtility()
    {
        drawersByAttributeType = new Dictionary<Type, PropertyDrawer>();
    }

    public static PropertyDrawer GetDrawer(Type attributeType)
    {
        return drawersByAttributeType[attributeType];
    }
}