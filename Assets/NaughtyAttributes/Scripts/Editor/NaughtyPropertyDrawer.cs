using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [InitializeOnLoad]
    public class NaughtyPropertyDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (PropertyUtility.GetTargetObjectWithProperty(property) != null)
            {
                if (!PropertyUtility.IsVisible(property))
                    return 0;
            }

            var handler = _ScriptAttributeUtility_GetPropertyHandler(property);

            return _PropertyHandler_GetHeight(handler, property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (PropertyUtility.GetTargetObjectWithProperty(property) == null)
            {
                DefaultOnGUI(position, property, label, true);
            }
            else
            {
                NaughtyEditorGUI.PropertyField_Implementation(position, property, true, DefaultOnGUI);
            }
        }

        static void DefaultOnGUI(Rect position, SerializedProperty property, GUIContent label, bool includeChildren)
        {
            var handler = _ScriptAttributeUtility_GetPropertyHandler(property);

            _PropertyHandler_OnGUI(handler, position, property, label, includeChildren);
        }


        const BindingFlags kAnyStaticMemberBindingFlags = BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        const BindingFlags kAnyInstanceMemberBindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        // UnityEditor.PropertyHandler is an internal type implementing default logic for many editor GUI operations
        private static Type _PropertyHandlerType;

        // UnityEditor.ScriptAttributeUtility allows us to retreive an instance of UnityEditor.PropertyHandler for a SerializedProperty
        // This is what Unity calls
        private static Type _ScriptAttributeUtilityType;

        // This method returns an instance of PropertyHandler for a SerializedProperty
        private static Func<SerializedProperty, object> _ScriptAttributeUtility_GetPropertyHandler;

        private static Func<object, SerializedProperty, GUIContent, float> _PropertyHandler_GetHeight;
        private static Func<object, Rect, SerializedProperty, GUIContent, bool, bool> _PropertyHandler_OnGUI;

        static NaughtyPropertyDrawer()
        {
            ResolveInternalUnityEditorTypes();
            ResolveInternalUnityEditorMethods();

            SetFallbackPropertyDrawer();
            HackDrawerTypeForTypeDictionary();
        }

        static void ResolveInternalUnityEditorTypes()
        {
            var unityEditorAssemblyTypes = typeof(UnityEditor.EditorGUI).Assembly.GetTypes();

            _PropertyHandlerType = unityEditorAssemblyTypes.First(t => t.FullName == "UnityEditor.PropertyHandler");
            _ScriptAttributeUtilityType = unityEditorAssemblyTypes.First(t => t.FullName == "UnityEditor.ScriptAttributeUtility");
        }

        static void ResolveInternalUnityEditorMethods()
        {
            _ScriptAttributeUtility_GetPropertyHandler = (Func<SerializedProperty, object>)
                _ScriptAttributeUtilityType
                    .GetMethod("GetHandler", kAnyStaticMemberBindingFlags)
                    .CreateDelegate(typeof(Func<SerializedProperty, object>), null);

            Compile_PropertyHandler_GetHeight();
            Compile_PropertyHandler_OnGUI();
        }

        static void SetFallbackPropertyDrawer()
        {
            // There is two 'hacks' to ensure Unity calls the NaughtyPropertyDrawer
            // First - s_SharedNullHandler's property drawers are set to it - this is the fallback Unity uses.
            // Second - the 'drawer resolver' dictionary is modified in a way, that for any type Unity has not got a custom drawer registered, it will fallback to NaughtyPropertyDrawer.
            // Both appear necessary to handle all cases of Unity resolving drawers for any type (except the ones with explicit custom drawers).

            var ScriptAttributeUtility_sharedNullHandlerField = _ScriptAttributeUtilityType.GetField("s_SharedNullHandler", kAnyStaticMemberBindingFlags);

            var sharedNullHandler = ScriptAttributeUtility_sharedNullHandlerField.GetValue(null);

#if UNITY_2021
            // In 2021, the handler uses a collection of drawers instead of a single drawer
            // ScriptAttributeUtility.s_SharedNullHandler.m_PropertyDrawers
            var PropertyHandler_m_PropertyDrawersField = _PropertyHandlerType.GetField("m_PropertyDrawers", kAnyInstanceMemberBindingFlags);

            var propertyDrawers = new List<PropertyDrawer> { new NaughtyPropertyDrawer() };

            PropertyHandler_m_PropertyDrawersField.SetValue(sharedNullHandler, propertyDrawers);
#else
            // ScriptAttributeUtility.s_SharedNullHandler.m_PropertyDrawer
            var PropertyHandler_m_PropertyDrawerField = _PropertyHandlerType.GetField("m_PropertyDrawer", kAnyInstanceMemberBindingFlags);

            PropertyHandler_m_PropertyDrawerField.SetValue(sharedNullHandler, new NaughtyPropertyDrawer());
#endif
        }

        static void HackDrawerTypeForTypeDictionary()
        {
            // The hack works as follows:
            // The dictionary's comparer is set to a special comparer that will:
            //   for any type already present in the dictionary, perform standard comparision
            //   for a type not present in the dictionary, fallback always to returning the '0' value, which returns the 'NaughtyPropertyDrawer' value

            // Invoke the initialization method, as the dictionary may not have yet been created
            _ScriptAttributeUtilityType.GetMethod("BuildDrawerTypeForTypeDictionary", kAnyStaticMemberBindingFlags).Invoke(null, Array.Empty<object>());

            // Get the dictionary field, and value
            var ScriptAttributeUtility_s_DrawerTypeForTypeField = _ScriptAttributeUtilityType.GetField("s_DrawerTypeForType", kAnyStaticMemberBindingFlags);
            var drawerTypeForTypeDict = (IDictionary) ScriptAttributeUtility_s_DrawerTypeForTypeField.GetValue(null);

            // Resolve the 'value' type from the dictionary and its fields
            var drawerKeySetType = _ScriptAttributeUtilityType.GetNestedType("DrawerKeySet", BindingFlags.NonPublic);
            var drawerKeySet_drawerField = drawerKeySetType.GetField("drawer"); // drawer is type of the property drawer
            var drawerKeySet_typeField = drawerKeySetType.GetField("type"); // type is the handled type

            // Store all handled types in a hash set, for fast lookup in the comparer
            // Unity should populate the dictionary with all possible types, including derived types for 'base' drawers
            var handledTypes = new HashSet<Type>();

            foreach (var value in drawerTypeForTypeDict.Values)
            {
                var type = (Type) drawerKeySet_typeField.GetValue(value);

                handledTypes.Add(type);
            }

            var dictType = drawerTypeForTypeDict.GetType();

            // Resolve the internal comparer field
            var comparerField =
                dictType.GetField("comparer", kAnyInstanceMemberBindingFlags) // .NET Framework
                ?? dictType.GetField("_comparer", kAnyInstanceMemberBindingFlags); // .NET Standard

            // Set the comparer to our faker
            comparerField.SetValue(drawerTypeForTypeDict, new FallbackToNaughtyPropertyDrawerComparer(handledTypes));

            // Finally, add the indicator type to the dictionary
            var drawerKeySetInstance = Activator.CreateInstance(drawerKeySetType);
            drawerKeySet_drawerField.SetValue(drawerKeySetInstance, typeof(NaughtyPropertyDrawer));
            drawerKeySet_typeField.SetValue(drawerKeySetInstance, typeof(FallbackIndicatorType));
            drawerTypeForTypeDict[typeof(FallbackIndicatorType)] = drawerKeySetInstance;
        }

        // This is used as an indicator, so the comparer knows when we are faking the result
        class FallbackIndicatorType
        { }

        class FallbackToNaughtyPropertyDrawerComparer : IEqualityComparer<Type>
        {
            HashSet<Type> _handledTypes;

            public FallbackToNaughtyPropertyDrawerComparer(HashSet<Type> handledTypes)
            {
                _handledTypes = handledTypes;
            }

            public bool Equals(Type x, Type y)
            {
                if (x == typeof(FallbackIndicatorType) || y == typeof(FallbackIndicatorType))
                    return true;

                return x == y;
            }

            public int GetHashCode(Type obj)
            {
                if (obj != typeof(FallbackIndicatorType))
                {
                    if (_handledTypes.Contains(obj))
                        return obj.GetHashCode();
                }

                // 0 means fake result
                return 0;
            }
        }

        static void Compile_PropertyHandler_GetHeight()
        {
            var paramHandler = Expression.Parameter(typeof(object));
            var paramProperty = Expression.Parameter(typeof(SerializedProperty));
            var paramLabel = Expression.Parameter(typeof(GUIContent));

            var method =
                _PropertyHandlerType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .First(m => m.Name == "GetHeight" && m.GetParameters().Length == 3);

            _PropertyHandler_GetHeight =
                Expression.Lambda<Func<object, SerializedProperty, GUIContent, float>>(
                    Expression.Call(
                        Expression.TypeAs(paramHandler, _PropertyHandlerType), method,
                        paramProperty,
                        paramLabel,
                        Expression.Constant(true)),
                    new[] { paramHandler, paramProperty, paramLabel })
                    .Compile();
        }

        static void Compile_PropertyHandler_OnGUI()
        {
            var paramHandler = Expression.Parameter(typeof(object));
            var paramPosition = Expression.Parameter(typeof(Rect));
            var paramProperty = Expression.Parameter(typeof(SerializedProperty));
            var paramLabel = Expression.Parameter(typeof(GUIContent));
            var paramIncludeChildren = Expression.Parameter(typeof(bool));

            var method =
                _PropertyHandlerType
                    .GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    .First(m => m.Name == "OnGUI" && m.GetParameters().Length == 4);

            _PropertyHandler_OnGUI =
                Expression.Lambda<Func<object, Rect, SerializedProperty, GUIContent, bool, bool>>(
                    Expression.Call(
                         Expression.TypeAs(paramHandler, _PropertyHandlerType), method,
                        paramPosition,
                        paramProperty,
                        paramLabel,
                        paramIncludeChildren),
                    new[] { paramHandler, paramPosition, paramProperty, paramLabel, paramIncludeChildren })
                    .Compile();
        }
    }
}
