using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyMeta
    {
        public abstract void ApplyPropertyMeta(SerializedProperty property, MetaAttribute metaAttribute);

        public virtual void ClearCache()
        {

        }
    }
}
