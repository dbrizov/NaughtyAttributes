// This class is auto generated

using System;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    public static class MetaDatabase
    {
        private static Dictionary<Type, PropertyMeta> metasByAttributeType;

        static MetaDatabase()
        {
            metasByAttributeType = new Dictionary<Type, PropertyMeta>();
            metasByAttributeType[typeof(BlankSpaceAttribute)] = new BlankSpacePropertyMeta();
metasByAttributeType[typeof(SectionAttribute)] = new SectionPropertyMeta();

        }

        public static PropertyMeta GetMetaForAttribute(Type attributeType)
        {
            PropertyMeta meta;
            if (metasByAttributeType.TryGetValue(attributeType, out meta))
            {
                return meta;
            }
            else
            {
                return null;
            }
        }

        public static void ClearCache()
        {
            foreach (var kvp in metasByAttributeType)
            {
                kvp.Value.ClearCache();
            }
        }
    }
}

