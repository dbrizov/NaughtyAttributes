using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ReadOnlyAttribute))]
    public class ReadOnlyPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
        	EditorGUI.BeginDisabledGroup(true);
			EditorGUILayout.PropertyField(property, true);
			EditorGUI.EndDisabledGroup();
        }
    }
}
