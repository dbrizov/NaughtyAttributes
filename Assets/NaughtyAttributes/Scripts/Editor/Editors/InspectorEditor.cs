using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(MonoBehaviour), true)]
    public class InspectorEditor : UnityEditor.Editor
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

            // Draw grouped fields
            IEnumerable<FieldInfo> groupedFields = fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0);
            IEnumerable<IGrouping<string, FieldInfo>> groups = groupedFields.GroupBy(f => (f.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name);

            //foreach (var group in groups)
            //{
            //    string groupName = group.Key;
            //    GUIStyle groupStyle = (group.First().GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Style;

            //    EditorGUILayout.BeginVertical(groupStyle);
            //    EditorGUILayout.LabelField(groupName, EditorStyles.boldLabel);

            //    this.DrawAndValidateFields(group);

            //    EditorGUILayout.EndVertical();
            //}

            // Draw non-grouped fields
            IEnumerable<FieldInfo> nonGroupedFields = fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length == 0);
            this.DrawAndValidateFields(nonGroupedFields);

            this.serializedObject.ApplyModifiedProperties();
        }

        private void DrawAndValidateFields(IEnumerable<FieldInfo> fields)
        {
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
        }
    }
}
