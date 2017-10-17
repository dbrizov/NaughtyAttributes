using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class InspectorEditor : Editor
{
    private SerializedProperty script;

    private void OnEnable()
    {
        this.script = this.serializedObject.FindProperty("m_Script");
    }

    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        GUI.enabled = false;
        EditorGUILayout.PropertyField(this.script);
        GUI.enabled = true;

        FieldInfo[] fields = this.target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        foreach (var field in fields)
        {
            // Validate the field
            ValidatorAttribute[] validatorAttributes = (ValidatorAttribute[])field.GetCustomAttributes(typeof(ValidatorAttribute), true);
            if (validatorAttributes.Length > 0)
            {
                foreach (var attribute in validatorAttributes)
                {
                    PropertyValidator validator = ValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                    validator.ValidateProperty(this.serializedObject.FindProperty(field.Name));
                }
            }

            // Draw the field
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                PropertyDrawer drawer = DrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                drawer.DrawProperty(this.serializedObject.FindProperty(field.Name));
            }
            else
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name));
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}