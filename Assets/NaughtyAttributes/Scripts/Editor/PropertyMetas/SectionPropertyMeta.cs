using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyMeta(typeof(SectionAttribute))]
    public class SectionPropertyMeta : PropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, MetaAttribute metaAttribute)
        {
            SectionAttribute sectionAttribute = (SectionAttribute)metaAttribute;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(sectionAttribute.SectionLabel, EditorStyles.boldLabel);
        }
    }
}
