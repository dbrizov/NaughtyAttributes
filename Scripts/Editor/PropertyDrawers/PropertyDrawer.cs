using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyDrawer
    {
        public abstract void DrawProperty(SerializedProperty property);

        public virtual void ClearCache()
        {

        }
    }
}
