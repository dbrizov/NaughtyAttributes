using UnityEngine;
using UnityEditor;
using System.Reflection;
using System.Linq;

[CanEditMultipleObjects]
[CustomEditor(typeof(MonoBehaviour), true)]
public class InspectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        this.serializedObject.Update();

        FieldInfo[] fields = this.target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);

        // If we have fields with custom drawers
        if (fields.Any(f => f.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0))
        {
            // Use the custom property drawer
        }
        else
        {
            // Use default drawer
            foreach (var field in fields)
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name));
            }
        }

        // Validate the fields
        foreach (var field in fields)
        {
            ValidatorAttribute[] validatorAttributes = (ValidatorAttribute[])field.GetCustomAttributes(typeof(ValidatorAttribute), true);
            if (validatorAttributes.Length > 0)
            {
                foreach (var attribute in validatorAttributes)
                {
                    PropertyValidator validator = ValidatorUtility.GetValidatorForAttribute(attribute.GetType());
                    validator.ValidateProperty(this.serializedObject.FindProperty(field.Name));
                }
            }
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}