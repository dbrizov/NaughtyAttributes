using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CanEditMultipleObjects]
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class InspectorEditor : UnityEditor.Editor
    {
        private SerializedProperty script;

        private IEnumerable<FieldInfo> fields;
        private HashSet<FieldInfo> groupedFields;
        private Dictionary<string, List<FieldInfo>> groupedFieldsByGroupName;
        private IEnumerable<FieldInfo> nonSerializedFields;
        private IEnumerable<PropertyInfo> nativeProperties;
        private IEnumerable<MethodInfo> methods;

        private Dictionary<string, SerializedProperty> serializedPropertiesByFieldName;

        private bool useDefaultInspector;

        private void OnEnable()
        {
            this.script = this.serializedObject.FindProperty("m_Script");

            // Cache serialized fields
            this.fields = ReflectionUtility.GetAllFields(this.target, f => this.serializedObject.FindProperty(f.Name) != null);

            // If there are no NaughtyAttributes use default inspector
            if (this.fields.All(f => f.GetCustomAttributes(typeof(NaughtyAttribute), true).Length == 0))
            {
                this.useDefaultInspector = true;
            }
            else
            {
                this.useDefaultInspector = false;

                // Cache grouped fields
                this.groupedFields = new HashSet<FieldInfo>(this.fields.Where(f => f.GetCustomAttributes(typeof(GroupAttribute), true).Length > 0));

                // Cache grouped fields by group name
                this.groupedFieldsByGroupName = new Dictionary<string, List<FieldInfo>>();
                foreach (var groupedField in this.groupedFields)
                {
                    string groupName = (groupedField.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name;

                    if (this.groupedFieldsByGroupName.ContainsKey(groupName))
                    {
                        this.groupedFieldsByGroupName[groupName].Add(groupedField);
                    }
                    else
                    {
                        this.groupedFieldsByGroupName[groupName] = new List<FieldInfo>()
                        {
                            groupedField
                        };
                    }
                }

                // Cache serialized properties by field name
                this.serializedPropertiesByFieldName = new Dictionary<string, SerializedProperty>();
                foreach (var field in this.fields)
                {
                    this.serializedPropertiesByFieldName[field.Name] = this.serializedObject.FindProperty(field.Name);
                }
            }

            // Cache non-serialized fields
            this.nonSerializedFields = ReflectionUtility.GetAllFields(
                this.target, f => f.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0 && this.serializedObject.FindProperty(f.Name) == null);

            // Cache the native properties
            this.nativeProperties = ReflectionUtility.GetAllProperties(
                this.target, p => p.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);

            // Cache methods with DrawerAttribute
            this.methods = ReflectionUtility.GetAllMethods(
                this.target, m => m.GetCustomAttributes(typeof(DrawerAttribute), true).Length > 0);
        }

        private void OnDisable()
        {
            PropertyDrawerDatabase.ClearCache();
        }

        public override void OnInspectorGUI()
        {
            if (this.useDefaultInspector)
            {
                this.DrawDefaultInspector();
            }
            else
            {
                this.serializedObject.Update();

                if (this.script != null)
                {
                    GUI.enabled = false;
                    EditorGUILayout.PropertyField(this.script);
                    GUI.enabled = true;
                }

                // Draw fields
                HashSet<string> drawnGroups = new HashSet<string>();
                foreach (var field in this.fields)
                {
                    if (this.groupedFields.Contains(field))
                    {
                        // Draw grouped fields
                        string groupName = (field.GetCustomAttributes(typeof(GroupAttribute), true)[0] as GroupAttribute).Name;
                        if (!drawnGroups.Contains(groupName))
                        {
                            drawnGroups.Add(groupName);

                            PropertyGrouper grouper = this.GetPropertyGrouperForField(field);
                            if (grouper != null)
                            {
                                grouper.BeginGroup(groupName);

                                this.ValidateAndDrawFields(this.groupedFieldsByGroupName[groupName]);

                                grouper.EndGroup();
                            }
                            else
                            {
                                this.ValidateAndDrawFields(this.groupedFieldsByGroupName[groupName]);
                            }
                        }
                    }
                    else
                    {
                        // Draw non-grouped field
                        this.ValidateAndDrawField(field);
                    }
                }

                this.serializedObject.ApplyModifiedProperties();
            }

            // Draw non-serialized fields
            foreach (var field in this.nonSerializedFields)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)field.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                FieldDrawer drawer = FieldDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (drawer != null)
                {
                    drawer.DrawField(this.target, field);
                }
            }

            // Draw native properties
            foreach (var property in this.nativeProperties)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)property.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                NativePropertyDrawer drawer = NativePropertyDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (drawer != null)
                {
                    drawer.DrawNativeProperty(this.target, property);
                }
            }

            // Draw methods
            foreach (var method in this.methods)
            {
                DrawerAttribute drawerAttribute = (DrawerAttribute)method.GetCustomAttributes(typeof(DrawerAttribute), true)[0];
                MethodDrawer methodDrawer = MethodDrawerDatabase.GetDrawerForAttribute(drawerAttribute.GetType());
                if (methodDrawer != null)
                {
                    methodDrawer.DrawMethod(this.target, method);
                }
            }
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
            if (!ShouldDrawField(field))
            {
                return;
            }

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
                    validator.ValidateProperty(this.serializedPropertiesByFieldName[field.Name]);
                }
            }
        }

        private bool ShouldDrawField(FieldInfo field)
        {
            // Check if the field has draw conditions
            PropertyDrawCondition drawCondition = this.GetPropertyDrawConditionForField(field);
            if (drawCondition != null)
            {
                bool canDrawProperty = drawCondition.CanDrawProperty(this.serializedPropertiesByFieldName[field.Name]);
                if (!canDrawProperty)
                {
                    return false;
                }
            }

            // Check if the field has HideInInspectorAttribute
            HideInInspector[] hideInInspectorAttributes = (HideInInspector[])field.GetCustomAttributes(typeof(HideInInspector), true);
            if (hideInInspectorAttributes.Length > 0)
            {
                return false;
            }

            return true;
        }

        private void DrawField(FieldInfo field)
        {        
            EditorGUI.BeginChangeCheck();
            PropertyDrawer drawer = this.GetPropertyDrawerForField(field);
            if (drawer != null)
            {
                drawer.DrawProperty(this.serializedPropertiesByFieldName[field.Name]);
            }
            else
            {
                EditorDrawUtility.DrawPropertyField(this.serializedPropertiesByFieldName[field.Name]);
            }

            if (EditorGUI.EndChangeCheck())
            {
                OnValueChangedAttribute[] onValueChangedAttributes = (OnValueChangedAttribute[])field.GetCustomAttributes(typeof(OnValueChangedAttribute), true);
                foreach (var onValueChangedAttribute in onValueChangedAttributes)
                {
                    PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(onValueChangedAttribute.GetType());
                    if (meta != null)
                    {
                        meta.ApplyPropertyMeta(this.serializedPropertiesByFieldName[field.Name], onValueChangedAttribute);
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

            Array.Sort(metaAttributes, (x, y) =>
            {
                return x.Order - y.Order;
            });

            foreach (var metaAttribute in metaAttributes)
            {
                PropertyMeta meta = PropertyMetaDatabase.GetMetaForAttribute(metaAttribute.GetType());
                if (meta != null)
                {
                    meta.ApplyPropertyMeta(this.serializedPropertiesByFieldName[field.Name], metaAttribute);
                }
            }
        }

        private PropertyDrawer GetPropertyDrawerForField(FieldInfo field)
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

        private PropertyGrouper GetPropertyGrouperForField(FieldInfo field)
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

        private PropertyDrawCondition GetPropertyDrawConditionForField(FieldInfo field)
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
