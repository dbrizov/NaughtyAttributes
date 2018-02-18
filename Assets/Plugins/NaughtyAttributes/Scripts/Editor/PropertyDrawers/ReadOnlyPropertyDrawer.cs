using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            GUI.enabled = false;
            EditorDrawUtility.DrawPropertyField(property);
            GUI.enabled = true;
        }
    }
}
