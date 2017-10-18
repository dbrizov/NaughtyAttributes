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
                    SerializedProperty property = this.serializedObject.FindProperty(field.Name);
                    if (property != null)
                    {
                        PropertyValidator validator = ValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                        validator.ValidateProperty(property);
                    }
                }
            }

            // Draw the field
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                SerializedProperty property = this.serializedObject.FindProperty(field.Name);
                if (property != null)
                {
                    PropertyDrawer drawer = DrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                    drawer.DrawProperty(property);
                }
            }
            else
            {

                SerializedProperty property = this.serializedObject.FindProperty(field.Name);
                if (property != null)
                {
                    EditorGUILayout.PropertyField(property);
                }
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}