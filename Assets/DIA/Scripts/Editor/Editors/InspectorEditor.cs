using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

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

        IEnumerable<FieldInfo> fields =
            this.target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
            .Where(f => this.serializedObject.FindProperty(f.Name) != null);

        foreach (var field in fields)
        {
            // Validate the field
            ValidatorAttribute[] validatorAttributes = (ValidatorAttribute[])field.GetCustomAttributes(typeof(ValidatorAttribute), true);
            if (validatorAttributes.Length > 0)
            {
                foreach (var attribute in validatorAttributes)
                {
                    PropertyValidator validator = ValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                    if (validator != null)
                    {
                        validator.ValidateProperty(this.serializedObject.FindProperty(field.Name));
                    }
                }
            }

            // Draw the field
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                PropertyDrawer drawer = DrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                if (drawer != null)
                {
                    drawer.DrawProperty(this.serializedObject.FindProperty(field.Name));
                }
                else
                {
                    EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name));
                }
            }
            else
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name));
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}