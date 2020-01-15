using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(InputAxisAttribute))]
    public class InputAxisPropertyDrawer : PropertyDrawer
    {
        private static readonly string AssetPath = Path.Combine("ProjectSettings", "InputManager.asset");
        private const string AxesPropertyPath = "m_Axes";
        private const string NamePropertyPath = "m_Name";

        public override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyType != SerializedPropertyType.String)
            {
                EditorGUILayout.HelpBox($"{property.name} must be a string.", MessageType.Warning);
                return;
            }

            var inputManagerAsset = AssetDatabase.LoadAssetAtPath(AssetPath, typeof(object));
            var inputManager = new SerializedObject(inputManagerAsset);

            var axesProperty = inputManager.FindProperty(AxesPropertyPath);
            var axesSet = new HashSet<string>();

            for (var i = 0; i < axesProperty.arraySize; i++)
            {
                var axis = axesProperty.GetArrayElementAtIndex(i).FindPropertyRelative(NamePropertyPath).stringValue;
                axesSet.Add(axis);
            }

            var axes = axesSet.ToArray();
            var index = Array.IndexOf(axes, property.stringValue);
            index = Mathf.Clamp(index, 0, axes.Length - 1);

            var newIndex = EditorGUILayout.Popup(property.displayName, index, axes);
            property.stringValue = axes[newIndex];
        }
    }
}
