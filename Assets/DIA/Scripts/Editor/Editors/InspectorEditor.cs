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

        // If we have fields with validators
        if (fields.Any(f => f.GetCustomAttributes(typeof(ValidatorAttribute), true).Length > 0))
        {
            Debug.Log("Validate");
            // Validate the fields
        }
        else
        {
            Debug.Log("Don't Validate");
        }

        this.serializedObject.ApplyModifiedProperties();
    }
}