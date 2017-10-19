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

            DrawerDatabase.ClearCache();
            GrouperDatabase.ClearCache();
            ValidatorDatabase.ClearCache();
            DrawConditionDatabase.ClearCache();
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

            // Validate and draw grouped fields
            IEnumerable<FieldInfo> groupedFields = fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0);
            IEnumerable<IGrouping<string, FieldInfo>> groups = groupedFields.GroupBy(f => (f.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name);

            foreach (var group in groups)
            {
                PropertyGrouper grouper = this.GetGrouperForField(group.First());
                if (grouper != null)
                {
                    grouper.BeginGroup(group.Key);

                    this.ValidateAndDrawFields(group);

                    grouper.EndGroup();
                }
                else
                {
                    this.ValidateAndDrawFields(group);
                }
            }

            // Validate and draw non-grouped fields
            IEnumerable<FieldInfo> nonGroupedFields = fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length == 0);
            this.ValidateAndDrawFields(nonGroupedFields);

            this.serializedObject.ApplyModifiedProperties();
        }

        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields)
            {
                this.ValidateField(field);
                this.DrawField(field);
            }
        }

        private void ValidateField(FieldInfo field)
        {
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
        }

        private void DrawField(FieldInfo field)
        {
            PropertyDrawCondition drawCondition = this.GetDrawConditionForField(field);
            if (drawCondition != null)
            {
                bool canDrawProperty = drawCondition.CanDrawProperty(this.serializedObject.FindProperty(field.Name));
                if (!canDrawProperty)
                {
                    return;
                }
            }

            PropertyDrawer drawer = this.GetDrawerForField(field);
            if (drawer != null)
            {
                drawer.DrawProperty(this.serializedObject.FindProperty(field.Name));
            }
            else
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name), true);
            }
        }

        private PropertyDrawer GetDrawerForField(FieldInfo field)
        {
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                PropertyDrawer drawer = DrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
                return drawer;
            }
            else
            {
                return null;
            }
        }

        private PropertyGrouper GetGrouperForField(FieldInfo field)
        {
            GroupAttribute[] groupAttributes = (GroupAttribute[])field.GetCustomAttributes(typeof(GroupAttribute), true);
            if (groupAttributes.Length > 0)
            {
                PropertyGrouper grouper = GrouperDatabase.GetGrouperForAttribute(groupAttributes[0].GetType());
                return grouper;
            }
            else
            {
                return null;
            }
        }

        private PropertyDrawCondition GetDrawConditionForField(FieldInfo field)
        {
            DrawConditionAttribute[] drawConditionAttributes = (DrawConditionAttribute[])field.GetCustomAttributes(typeof(DrawConditionAttribute), true);
            if (drawConditionAttributes.Length > 0)
            {
                PropertyDrawCondition drawCondition = DrawConditionDatabase.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
                return drawCondition;
            }
            else
            {
                return null;
            }
        }
    }
}
