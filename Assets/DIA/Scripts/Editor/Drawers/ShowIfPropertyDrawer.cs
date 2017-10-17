using UnityEditor;

[PropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfPropertyDrawer : PropertyDrawer
{
    protected override void DrawPropertyImplementation(SerializedProperty property)
    {
        EditorGUILayout.PropertyField(property);
    }
}