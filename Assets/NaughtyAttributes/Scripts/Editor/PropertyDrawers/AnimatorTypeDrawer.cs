using UnityEditor;
using UnityEngine;
using System.Reflection;
using System.Collections.Generic;


namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(AnimatorTypeAttribute))]
    public class AnimatorTypeDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                AnimatorTypeAttribute typeAttribute = PropertyUtility.GetAttribute<AnimatorTypeAttribute>(property);
                UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

                FieldInfo fieldInfo = ReflectionUtility.GetField(target, property.name);
                FieldInfo animatorInfo = ReflectionUtility.GetField(target, typeAttribute.FieldName);

                Animator animator = (Animator)animatorInfo.GetValue(target);

                if (animatorInfo == null || !animator)
                {
                    this.DrawWarningBox(string.Format("Cannot find a values field with given name"));
                    EditorGUILayout.PropertyField(property, true);
                } 
                    else
                {

                    List<string> allNames = new List<string>();

                    string propertyValue = property.stringValue;
                    int index = 0;

                    for (int i = 0; i < animator.parameters.Length; i++)
                    {
                        if (animator.parameters[i].type == typeAttribute.Type)
                        {
                            allNames.Add(animator.parameters[i].name);

                            if (animator.parameters[i].name == propertyValue)
                            {
                                index = allNames.Count - 1;
                            }
                        }
                    }

                    string[] results = allNames.ToArray();
                    DrawDropdown(target, fieldInfo, property.displayName, index, results, results);
                }

            }
        }
        private void DrawDropdown(UnityEngine.Object target, FieldInfo fieldInfo, string label, int selectedValueIndex, object[] values, string[] displayOptions)
        {
            EditorGUI.BeginChangeCheck();

            int newIndex = EditorGUILayout.Popup(label, selectedValueIndex, displayOptions);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(target, "Dropdown");
                fieldInfo.SetValue(target, values[newIndex]);
            }
        }

        private void DrawWarningBox(string message)
        {
            EditorGUILayout.HelpBox(message, MessageType.Warning);
            Debugger.LogWarning(message);
        }
    }
}
