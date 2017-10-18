using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyDrawer
    {
        public void DrawProperty(SerializedProperty property)
        {
            this.DrawPropertyImplementation(property);
        }

        protected abstract void DrawPropertyImplementation(SerializedProperty property);
    }
}
