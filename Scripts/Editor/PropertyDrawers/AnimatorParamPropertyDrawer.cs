using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [CustomPropertyDrawer(typeof(AnimatorParamAttribute))]
    public class AnimatorParamPropertyDrawer : PropertyDrawerBase
    {
        private const string InvalidAnimatorControllerWarningMessage = "Target animator controller is null";
        private const string InvalidTypeWarningMessage = "{0} must be an int or a string";

        protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
        {
            AnimatorParamAttribute animatorParamAttribute = PropertyUtility.GetAttribute<AnimatorParamAttribute>(property);
            bool validAnimatorController = GetAnimatorController(property, animatorParamAttribute.AnimatorName) != null;
            bool validPropertyType = property.propertyType == SerializedPropertyType.Integer || property.propertyType == SerializedPropertyType.String;

            return (validAnimatorController && validPropertyType)
                ? GetPropertyHeight(property)
                : GetPropertyHeight(property) + GetHelpBoxHeight();
        }

        protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(rect, label, property);

            AnimatorParamAttribute animatorParamAttribute = PropertyUtility.GetAttribute<AnimatorParamAttribute>(property);

            AnimatorController animatorController = GetAnimatorController(property, animatorParamAttribute.AnimatorName);
            if (animatorController == null)
            {
                DrawDefaultPropertyAndHelpBox(rect, property, InvalidAnimatorControllerWarningMessage, MessageType.Warning);
                return;
            }

            int parametersCount = animatorController.parameters.Length;
            List<AnimatorControllerParameter> animatorParameters = new List<AnimatorControllerParameter>(parametersCount);
            for (int i = 0; i < parametersCount; i++)
            {
                AnimatorControllerParameter parameter = animatorController.parameters[i];
                if (animatorParamAttribute.AnimatorParamType == null || parameter.type == animatorParamAttribute.AnimatorParamType)
                {
                    animatorParameters.Add(parameter);
                }
            }

            switch (property.propertyType)
            {
                case SerializedPropertyType.Integer:
                    DrawPropertyForInt(rect, property, label, animatorParameters);
                    break;
                case SerializedPropertyType.String:
                    DrawPropertyForString(rect, property, label, animatorParameters);
                    break;
                default:
                    DrawDefaultPropertyAndHelpBox(rect, property, string.Format(InvalidTypeWarningMessage, property.name), MessageType.Warning);
                    break;
            }

            EditorGUI.EndProperty();
        }

        private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerParameter> animatorParameters)
        {
            int paramNameHash = property.intValue;
            int index = 0;

            for (int i = 0; i < animatorParameters.Count; i++)
            {
                if (paramNameHash == animatorParameters[i].nameHash)
                {
                    index = i + 1; // +1 because the first option is reserved for (None)
                    break;
                }
            }

            string[] displayOptions = GetDisplayOptions(animatorParameters);

            int newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
            int newValue = newIndex == 0 ? 0 : animatorParameters[newIndex - 1].nameHash;

            if (property.intValue != newValue)
            {
                property.intValue = newValue;
            }
        }

        private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerParameter> animatorParameters)
        {
            string paramName = property.stringValue;
            int index = 0;

            for (int i = 0; i < animatorParameters.Count; i++)
            {
                if (paramName.Equals(animatorParameters[i].name, System.StringComparison.Ordinal))
                {
                    index = i + 1; // +1 because the first option is reserved for (None)
                    break;
                }
            }

            string[] displayOptions = GetDisplayOptions(animatorParameters);

            int newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
            string newValue = newIndex == 0 ? null : animatorParameters[newIndex - 1].name;

            if (!property.stringValue.Equals(newValue, System.StringComparison.Ordinal))
            {
                property.stringValue = newValue;
            }
        }

        private static string[] GetDisplayOptions(List<AnimatorControllerParameter> animatorParams)
        {
            string[] displayOptions = new string[animatorParams.Count + 1];
            displayOptions[0] = "(None)";

            for (int i = 0; i < animatorParams.Count; i++)
            {
                displayOptions[i + 1] = animatorParams[i].name;
            }

            return displayOptions;
        }

        private static AnimatorController GetAnimatorController(SerializedProperty property, string animatorName)
        {
            object target = PropertyUtility.GetTargetObjectWithProperty(property);

            static Animator ResolveAnimator(object obj, string memberName)
            {
                var field = ReflectionUtility.GetField(obj, memberName);
                if (field != null && field.FieldType == typeof(Animator))
                {
                    return field.GetValue(obj) as Animator;
                }

                var prop = ReflectionUtility.GetProperty(obj, memberName);
                if (prop != null && prop.PropertyType == typeof(Animator))
                {
                    return prop.GetValue(obj) as Animator;
                }

                var getter = ReflectionUtility.GetMethod(obj, memberName);
                if (getter != null && getter.ReturnType == typeof(Animator) && getter.GetParameters().Length == 0)
                {
                    return getter.Invoke(obj, null) as Animator;
                }

                return null;
            }

            var animator = ResolveAnimator(target, animatorName);
            if (animator == null) return null;

            var runtimeAnimatorController = animator.runtimeAnimatorController;
            // Handle both regular controllers and override controllers
            if (runtimeAnimatorController is AnimatorController animatorController)
            {
                return animatorController;
            }

            if (runtimeAnimatorController is AnimatorOverrideController animatorOverrideController)
            {
                // The base controller holds the parameters
                var baseCtrl = animatorOverrideController.runtimeAnimatorController as AnimatorController;
                return baseCtrl;
            }

            return null;
        }
    }
}
