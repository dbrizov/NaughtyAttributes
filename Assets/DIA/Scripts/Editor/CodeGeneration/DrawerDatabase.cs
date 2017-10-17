// This class is auto generated

using System;
using System.Collections.Generic;

public static class DrawerDatabase
{
    private static Dictionary<Type, PropertyDrawer> drawersByAttributeType;

    static DrawerDatabase()
    {
        drawersByAttributeType = new Dictionary<Type, PropertyDrawer>();
        drawersByAttributeType[typeof(ShowIfAttribute)] = new ShowIfPropertyDrawer();

    }

    public static PropertyDrawer GetDrawerForAttribute(Type attributeType)
    {
        return drawersByAttributeType[attributeType];
    }
}

