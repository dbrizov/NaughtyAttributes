#if UNITY_2021_3_OR_NEWER
using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEditor;

namespace NaughtyAttributes
{
    public abstract class SerializedReferencePropertyDrawerBase : PropertyDrawer
    {
        static float lineHeight => EditorGUIUtility.singleLineHeight;
        static float verticalSpacing => EditorGUIUtility.standardVerticalSpacing;
        protected abstract System.Type DefaultType { get; }
        protected abstract System.Type BaseType { get; }

        System.Collections.Generic.List<Type> m_Types = new List<Type>();

        bool m_Initialized = false;

        /// <summary>
        /// If you override OnGUI or GetPropertyHeight, you should call this method first
        /// </summary>
        protected void Init()
        {
            if (m_Initialized)
                return;

            CollectTypes(m_Types);
            Initialize();

            m_Initialized = true;
        }

        /// <summary>
        /// Called after types are collected. Can be used to initialze custom fields
        /// </summary>
        protected virtual void Initialize()
        {

        }

        /// <summary>
        /// Called during initialization to collect types. Can be overriden to provide custom types
        /// </summary>
        /// <param name="list"></param>
        protected virtual void CollectTypes(List<Type> list)
        {
            var assemblies = System.AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (BaseType.IsAssignableFrom(type) && !type.IsGenericType && !type.IsAbstract && type.AssemblyQualifiedName != DefaultType.AssemblyQualifiedName)
                    {
                        m_Types.Add(type);
                    }
                }
            }
        }


        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            Init();

            EditorGUI.BeginProperty(position, label, property);

            var dropDownRect = position;
            dropDownRect.height = lineHeight;
            dropDownRect.x += EditorGUIUtility.labelWidth + verticalSpacing;
            dropDownRect.width -= EditorGUIUtility.labelWidth + verticalSpacing;

            DrawDropdpownContent(dropDownRect, property);

            EditorGUI.PropertyField(position, property, label, true);
            EditorGUI.EndProperty();
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            Init();
            return EditorGUI.GetPropertyHeight(property);
        }

        public void DrawDropdpownContent(Rect position, SerializedProperty property)
        {
            EditorGUI.BeginDisabledGroup(EditorApplication.isPlayingOrWillChangePlaymode || EditorApplication.isPaused);

            var oCol = GUI.contentColor;
            if (property.managedReferenceValue == null)
            {
                FillProperty(property, DefaultType);
            }

            var currentObject = property.managedReferenceValue;
            var currentSelectorType = currentObject.GetType();
            var currentContent = GetGUIContentForType(currentSelectorType);

            var rect = position;

            if (EditorGUI.DropdownButton(rect, currentContent, FocusType.Keyboard))
            {
                GenericMenu menu = new GenericMenu();
                for (var i = 0; i < m_Types.Count; i++)
                {
                    var type = m_Types[i];
                    var content = GetGUIContentForType(type);
                    bool isOn = currentContent.text == content.text;
                    menu.AddItem(content, currentContent.text == content.text, () =>
                    {
                        if (!isOn)
                        {
                            FillProperty(property, type);
                        }
                    });
                }

                menu.AddSeparator(null);

                var defaultContent = GetGUIContentForType(DefaultType);
                bool defaultIsOn = defaultContent.text == currentContent.text;
                menu.AddItem(GetGUIContentForType(DefaultType), defaultIsOn, () =>
                {
                    if (!defaultIsOn)
                    {
                        FillProperty(property, DefaultType);
                    }
                });
                menu.DropDown(rect);
            }

            GUI.contentColor = oCol;

            GUIContent GetGUIContentForType(System.Type type)
            {
                if (type.AssemblyQualifiedName == DefaultType.AssemblyQualifiedName)
                    return new GUIContent($"{type.Name} (Default)");
                return new GUIContent($"{type.Name}");
            }
            EditorGUI.EndDisabledGroup();
        }

        private static void FillProperty(SerializedProperty property, System.Type type)
        {
            var newAsset = Activator.CreateInstance(type);
            property.managedReferenceValue = newAsset;
            property.serializedObject.ApplyModifiedProperties();
        }
    }
}
#endif