using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class InspectorEditor : UnityEditor.Editor
    {
        private SerializedProperty script;

        private void OnEnable()
        {
            this.script = this.serializedObject.FindProperty("m_Script");
        }

        private void OnDisable()
        {
            PropertyMetaDatabase.ClearCache();
            PropertyDrawerDatabase.ClearCache();
            PropertyGrouperDatabase.ClearCache();
            PropertyValidatorDatabase.ClearCache();
            PropertyDrawConditionDatabase.ClearCache();

            MethodDrawerDatabase.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            if (this.script != null)
            {
                GUI.enabled = false;
                EditorGUILayout.PropertyField(this.script);
                GUI.enabled = true;
            }

            // Draw fields
            IEnumerable<FieldInfo> fields =
                this.target.GetType().GetFields(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(f => this.serializedObject.FindProperty(f.Name) != null);

            IEnumerable<FieldInfo> groupedFields = fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0);

            HashSet<string> groupNames = new HashSet<string>();

            foreach (var field in fields)
            {
                GroupAttribute[] groupAttributes = (GroupAttribute[])field.GetCustomAttributes(typeof(GroupAttribute), true);
                if (groupAttributes.Length > 0)
                {
                    // Draw grouped fields
                    string groupName = groupAttributes[0].Name;
                    if (!groupNames.Contains(groupName))
                    {
                        groupNames.Add(groupName);

                        IEnumerable<FieldInfo> fieldsInSameGroup = groupedFields
                            .Where(f => (f.GetCustomAttributes(typeof(GroupAttribute), true) as GroupAttribute[])[0].Name == groupName);

                        PropertyGrouper grouper = this.GetGrouperForField(field);
                        if (grouper != null)
                        {
                            grouper.BeginGroup(groupName);

                            this.ValidateAndDrawFields(fieldsInSameGroup);

                            grouper.EndGroup();
                        }
                        else
                        {
                            this.ValidateAndDrawFields(fieldsInSameGroup);
                        }
                    }
                }
                else
                {
                    // Draw non-grouped field
                    this.ValidateAndDrawField(field);
                }
            }

            // Draw methods
            IEnumerable<MethodInfo> methods =
                this.target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public)
                .Where(m => m.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);

            foreach (var method in methods)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)method.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                MethodDrawer methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (methodDrawer != null)
                {
                    methodDrawer.DrawMethod(this.serializedObject.targetObject, method);
                }
            }

            this.serializedObject.ApplyModifiedProperties();
        }

        private void ValidateAndDrawFields(IEnumerable<FieldInfo> fields)
        {
            foreach (var field in fields)
            {
                this.ValidateAndDrawField(field);
            }
        }

        private void ValidateAndDrawField(FieldInfo field)
        {
            this.ValidateField(field);
            this.ApplyFieldMeta(field);
            this.DrawField(field);
        }

        private void ValidateField(FieldInfo field)
        {
            ValidatorAttribute[] validatorAttributes = (ValidatorAttribute[])field.GetCustomAttributes(typeof(ValidatorAttribute), true);

            foreach (var attribute in validatorAttributes)
            {
                PropertyValidator validator = PropertyValidatorDatabase.GetValidatorForAttribute(attribute.GetType());
                if (validator != null)
                {
                    validator.ValidateProperty(this.serializedObject.FindProperty(field.Name));
                }
            }
        }

        private void DrawField(FieldInfo field)
        {
            // Check if the field has draw conditions
            PropertyDrawCondition drawCondition = this.GetDrawConditionForField(field);
            if (drawCondition != null)
            {
                bool canDrawProperty = drawCondition.CanDrawProperty(this.serializedObject.FindProperty(field.Name));
                if (!canDrawProperty)
                {
                    return;
                }
            }

            // Check if the field has HideInInspectorAttribute
            HideInInspector[] hideInInspectorAttributes = (HideInInspector[])field.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0)
            {
                return;
            }

            // Draw the field
            EditorGUI.BeginChangeCheck();
            PropertyDrawer drawer = this.GetDrawerForField(field);
            if (drawer != null)
            {
                drawer.DrawProperty(this.serializedObject.FindProperty(field.Name));
            }
            else
            {
                EditorGUILayout.PropertyField(this.serializedObject.FindProperty(field.Name), true);
            }

            if (EditorGUI.EndChangeCheck())
            {
                OnValueChangedAttribute[] onValueChangedAttributes = (OnValueChangedAttribute[])field.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (var onValueChangedAttribute in onValueChangedAttributes)
                {
                    PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (meta != null)
                    {
                        meta.ApplyPropertyMeta(this.serializedObject.FindProperty(field.Name), onValueChangedAttribute);
                    }
                }
            }
        }

        private void ApplyFieldMeta(FieldInfo field)
        {
            // Apply custom meta attributes
            MetaAttribute[] metaAttributes = field
                .GetCustomAttributes(typeof(MetaAttribute), true)
                .Where(attr => attr.GetType() != typeof(OnValueChangedAttribute))
                .Select(obj => obj as MetaAttribute)
                .ToArray();

            System.Array.Sort(metaAttributes, (x, y) =>
            {
                return x.Order - y.Order;
            });

            foreach (var metaAttribute in metaAttributes)
            {
                PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(metaAttribute.GetType());
                if (meta != null)
                {
                    meta.ApplyPropertyMeta(this.serializedObject.FindProperty(field.Name), metaAttribute);
                }
            }
        }

        private PropertyDrawer GetDrawerForField(FieldInfo field)
        {
            DrawerAttribute[] drawerAttributes = (DrawerAttribute[])field.GetCustomAttributes(typeof(DrawerAttribute), true);
            if (drawerAttributes.Length > 0)
            {
                PropertyDrawer drawer = PropertyDrawerDatabase.GetDrawerForAttribute(drawerAttributes[0].GetType());
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
                PropertyGrouper grouper = PropertyGrouperDatabase.GetGrouperForAttribute(groupAttributes[0].GetType());
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
                PropertyDrawCondition drawCondition = PropertyDrawConditionDatabase.GetDrawConditionForAttribute(drawConditionAttributes[0].GetType());
                return drawCondition;
            }
            else
            {
                return null;
            }
        }
    }
}
