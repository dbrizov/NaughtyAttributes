using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyMeta(typeof(BlankSpaceAttribute))]
    public class BlankSpacePropertyMeta : PropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, MetaAttribute metaAttribute)
        {
            EditorGUILayout.Space();
        }
    }
}
