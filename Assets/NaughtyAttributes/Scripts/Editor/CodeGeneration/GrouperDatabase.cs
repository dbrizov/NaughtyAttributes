// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class GrouperDatabase
    {
        private static Dictionary<Type, PropertyGrouper> groupersByAttributeType;

        static GrouperDatabase()
        {
            groupersByAttributeType = new Dictionary<Type, PropertyGrouper>();
            groupersByAttributeType[typeof(BoxGroupAttribute)] = new BoxGroupPropertyGrouper();

        }

        public static PropertyGrouper GetGrouperForAttribute(Type attributeType)
        {
            PropertyGrouper grouper;
            if (groupersByAttributeType.TryGetValue(attributeType, out grouper))
            {
                return grouper;
            }
            else
            {
                return null;
            }
        }

        public static void DisposeGroupers()
        {
            foreach (var kvp in groupersByAttributeType)
            {
                kvp.Value.Dispose();
            }
        }
    }
}

