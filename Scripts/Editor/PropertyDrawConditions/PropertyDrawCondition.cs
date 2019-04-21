using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyDrawCondition
    {
        public abstract bool CanDrawProperty(SerializedProperty property);
    }
}
