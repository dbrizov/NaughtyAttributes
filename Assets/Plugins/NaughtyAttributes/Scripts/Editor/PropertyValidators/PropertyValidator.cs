using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyValidator
    {
        public abstract void ValidateProperty(SerializedProperty property);
    }
}
