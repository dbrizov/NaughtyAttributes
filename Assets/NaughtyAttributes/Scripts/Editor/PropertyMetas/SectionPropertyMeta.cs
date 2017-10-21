using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyMeta(typeof(SectionAttribute))]
    public class SectionPropertyMeta : PropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property)
        {
            SectionAttribute sectionAttribute = PropertyUtility.GetAttributes<SectionAttribute>(property)[0];

            EditorGUILayout.Space();
            EditorGUILayout.LabelField(sectionAttribute.SectionLabel, EditorStyles.boldLabel);
        }
    }
}
