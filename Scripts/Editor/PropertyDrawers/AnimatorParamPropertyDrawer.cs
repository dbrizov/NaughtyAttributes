using System.Collections.Generic;
using System.Reflection;
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
			if (newIndex == 0)
			{
				property.intValue = 0;
			}
			else
			{
				property.intValue = animatorParameters[newIndex - 1].nameHash;
			}
		}

		private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, List<AnimatorControllerParameter> animatorParameters)
		{
			string paramName = property.stringValue;
			int index = 0;

			for (int i = 0; i < animatorParameters.Count; i++)
			{
				if (paramName == animatorParameters[i].name)
				{
					index = i + 1; // +1 because the first option is reserved for (None)
					break;
				}
			}

			string[] displayOptions = GetDisplayOptions(animatorParameters);

			int newIndex = EditorGUI.Popup(rect, label.text, index, displayOptions);
			if (newIndex == 0)
			{
				property.stringValue = null;
			}
			else
			{
				property.stringValue = animatorParameters[newIndex - 1].name;
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

			FieldInfo animatorFieldInfo = ReflectionUtility.GetField(target, animatorName);
			if (animatorFieldInfo != null &&
				animatorFieldInfo.FieldType == typeof(Animator))
			{
				Animator animator = animatorFieldInfo.GetValue(target) as Animator;
				if (animator != null)
				{
					AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			PropertyInfo animatorPropertyInfo = ReflectionUtility.GetProperty(target, animatorName);
			if (animatorPropertyInfo != null &&
				animatorPropertyInfo.PropertyType == typeof(Animator))
			{
				Animator animator = animatorPropertyInfo.GetValue(target) as Animator;
				if (animator != null)
				{
					AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			MethodInfo animatorGetterMethodInfo = ReflectionUtility.GetMethod(target, animatorName);
			if (animatorGetterMethodInfo != null &&
				animatorGetterMethodInfo.ReturnType == typeof(Animator) &&
				animatorGetterMethodInfo.GetParameters().Length == 0)
			{
				Animator animator = animatorGetterMethodInfo.Invoke(target, null) as Animator;
				if (animator != null)
				{
					AnimatorController animatorController = animator.runtimeAnimatorController as AnimatorController;
					return animatorController;
				}
			}

			return null;
		}
	}
}
