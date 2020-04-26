using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.Experimental.SceneManagement;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public static class NaughtyEditorGUI
	{
		public static void PropertyField_Layout(SerializedProperty property, bool includeChildren)
		{
			SpecialCaseDrawerAttribute specialCaseAttribute = PropertyUtility.GetAttribute<SpecialCaseDrawerAttribute>(property);
			if (specialCaseAttribute != null)
			{
				specialCaseAttribute.GetDrawer().OnGUI(property);
			}
			else
			{
				GUIContent label = new GUIContent(PropertyUtility.GetLabel(property));
				bool anyDrawerAttribute = PropertyUtility.GetAttributes<DrawerAttribute>(property).Any();

				if (!anyDrawerAttribute)
				{
					// Drawer attributes check for visibility, enableability and validator themselves,
					// so if a property doesn't have a DrawerAttribute we need to check for these explicitly

					// Check if visible
					bool visible = PropertyUtility.IsVisible(property);
					if (!visible)
					{
						return;
					}

					// Validate
					ValidatorAttribute[] validatorAttributes = PropertyUtility.GetAttributes<ValidatorAttribute>(property);
					foreach (var validatorAttribute in validatorAttributes)
					{
						validatorAttribute.GetValidator().ValidateProperty(property);
					}

					// Check if enabled and draw
					EditorGUI.BeginChangeCheck();
					bool enabled = PropertyUtility.IsEnabled(property);
					GUI.enabled = enabled;
					EditorGUILayout.PropertyField(property, label, includeChildren);
					GUI.enabled = true;

					// Call OnValueChanged callbacks
					if (EditorGUI.EndChangeCheck())
					{
						PropertyUtility.CallOnValueChangedCallbacks(property);
					}
				}
				else
				{
					// We don't need to check for enableIfAttribute
					EditorGUILayout.PropertyField(property, label, includeChildren);
				}
			}
		}

		public static float GetIndentLength(Rect sourceRect)
		{
			Rect indentRect = EditorGUI.IndentedRect(sourceRect);
			float indentLength = indentRect.x - sourceRect.x;

			return indentLength;
		}

		public static void BeginBoxGroup_Layout(string label = "")
		{
			EditorGUILayout.BeginVertical(GUI.skin.box);
			if (!string.IsNullOrEmpty(label))
			{
				EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
			}
		}

		public static void EndBoxGroup_Layout()
		{
			EditorGUILayout.EndVertical();
		}

		/// <summary>
		/// Creates a dropdown
		/// </summary>
		/// <param name="rect">The rect the defines the position and size of the dropdown in the inspector</param>
		/// <param name="serializedObject">The serialized object that is being updated</param>
		/// <param name="target">The target object that contains the dropdown</param>
		/// <param name="dropdownField">The field of the target object that holds the currently selected dropdown value</param>
		/// <param name="label">The label of the dropdown</param>
		/// <param name="selectedValueIndex">The index of the value from the values array</param>
		/// <param name="values">The values of the dropdown</param>
		/// <param name="displayOptions">The display options for the values</param>
		public static void Dropdown(
			Rect rect, SerializedObject serializedObject, object target, FieldInfo dropdownField,
			string label, int selectedValueIndex, object[] values, string[] displayOptions)
		{
			EditorGUI.BeginChangeCheck();

			int newIndex = EditorGUI.Popup(rect, label, selectedValueIndex, displayOptions);

			if (EditorGUI.EndChangeCheck())
			{
				Undo.RecordObject(serializedObject.targetObject, "Dropdown");

				// TODO: Problem with structs, because they are value type.
				// The solution is to make boxing/unboxing but unfortunately I don't know the compile time type of the target object
				dropdownField.SetValue(target, values[newIndex]);
			}
		}

		public static void Button(UnityEngine.Object target, MethodInfo methodInfo)
		{
			bool visible = ButtonUtility.IsVisible(target, methodInfo);
			if (!visible)
			{
				return;
			}

			if (methodInfo.GetParameters().All(p => p.IsOptional))
			{
				ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
				string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? ObjectNames.NicifyVariableName(methodInfo.Name) : buttonAttribute.Text;

				bool buttonEnabled = ButtonUtility.IsEnabled(target, methodInfo);

				EButtonEnableMode mode = buttonAttribute.SelectedEnableMode;
				buttonEnabled &=
					mode == EButtonEnableMode.Always ||
					mode == EButtonEnableMode.Editor && !Application.isPlaying ||
					mode == EButtonEnableMode.Playmode && Application.isPlaying;

				bool methodIsCoroutine = methodInfo.ReturnType == typeof(IEnumerator);
				if (methodIsCoroutine)
				{
					buttonEnabled &= (Application.isPlaying ? true : false);
				}

				EditorGUI.BeginDisabledGroup(!buttonEnabled);

				if (GUILayout.Button(buttonText))
				{
					object[] defaultParams = methodInfo.GetParameters().Select(p => p.DefaultValue).ToArray();
					IEnumerator methodResult = methodInfo.Invoke(target, defaultParams) as IEnumerator;

					if (!Application.isPlaying)
					{
						// Set target object and scene dirty to serialize changes to disk
						EditorUtility.SetDirty(target);

						PrefabStage stage = PrefabStageUtility.GetCurrentPrefabStage();
						if (stage != null)
						{
							// Prefab mode
							EditorSceneManager.MarkSceneDirty(stage.scene);
						}
						else
						{
							// Normal scene
							EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
						}
					}
					else if (methodResult != null && target is MonoBehaviour behaviour)
					{
						behaviour.StartCoroutine(methodResult);
					}
				}

				EditorGUI.EndDisabledGroup();
			}
			else
			{
				string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
				HelpBox_Layout(warning, MessageType.Warning, context: target, logToConsole: true);
			}
		}

		public static void NativeProperty_Layout(UnityEngine.Object target, PropertyInfo property)
		{
			object value = property.GetValue(target, null);

			if (value == null)
			{
				string warning = string.Format("{0} is null. {1} doesn't support reference types with null value", property.Name, typeof(ShowNativePropertyAttribute).Name);
				HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
			else if (!Field_Layout(value, property.Name))
			{
				string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyAttribute).Name, property.PropertyType.Name);
				HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
		}

		public static void NonSerializedField_Layout(UnityEngine.Object target, FieldInfo field)
		{
			object value = field.GetValue(target);

			if (value == null)
			{
				string warning = string.Format("{0} is null. {1} doesn't support reference types with null value", field.Name, typeof(ShowNonSerializedFieldAttribute).Name);
				HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
			else if (!Field_Layout(value, field.Name))
			{
				string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldAttribute).Name, field.FieldType.Name);
				HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
		}

		public static void HorizontalLine(Rect rect, float height, Color color)
		{
			rect.height = height;
			EditorGUI.DrawRect(rect, color);
		}

		public static void HelpBox(Rect rect, string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = false)
		{
			EditorGUI.HelpBox(rect, message, type);

			if (logToConsole)
			{
				DebugLogMessage(message, type, context);
			}
		}

		public static void HelpBox_Layout(string message, MessageType type, UnityEngine.Object context = null, bool logToConsole = false)
		{
			EditorGUILayout.HelpBox(message, type);

			if (logToConsole)
			{
				DebugLogMessage(message, type, context);
			}
		}

		public static bool Field_Layout(object value, string label)
		{
			GUI.enabled = false;

			bool isDrawn = true;
			Type valueType = value.GetType();

			if (valueType == typeof(bool))
			{
				EditorGUILayout.Toggle(label, (bool)value);
			}
			else if (valueType == typeof(int))
			{
				EditorGUILayout.IntField(label, (int)value);
			}
			else if (valueType == typeof(long))
			{
				EditorGUILayout.LongField(label, (long)value);
			}
			else if (valueType == typeof(float))
			{
				EditorGUILayout.FloatField(label, (float)value);
			}
			else if (valueType == typeof(double))
			{
				EditorGUILayout.DoubleField(label, (double)value);
			}
			else if (valueType == typeof(string))
			{
				EditorGUILayout.TextField(label, (string)value);
			}
			else if (valueType == typeof(Vector2))
			{
				EditorGUILayout.Vector2Field(label, (Vector2)value);
			}
			else if (valueType == typeof(Vector3))
			{
				EditorGUILayout.Vector3Field(label, (Vector3)value);
			}
			else if (valueType == typeof(Vector4))
			{
				EditorGUILayout.Vector4Field(label, (Vector4)value);
			}
			else if (valueType == typeof(Color))
			{
				EditorGUILayout.ColorField(label, (Color)value);
			}
			else if (valueType == typeof(Bounds))
			{
				EditorGUILayout.BoundsField(label, (Bounds)value);
			}
			else if (valueType == typeof(Rect))
			{
				EditorGUILayout.RectField(label, (Rect)value);
			}
			else if (typeof(UnityEngine.Object).IsAssignableFrom(valueType))
			{
				EditorGUILayout.ObjectField(label, (UnityEngine.Object)value, valueType, true);
			}
			else if (valueType.BaseType == typeof(Enum))
			{
				EditorGUILayout.EnumPopup(label, (Enum)value);
			}
			else
			{
				isDrawn = false;
			}

			GUI.enabled = true;

			return isDrawn;
		}

		private static void DebugLogMessage(string message, MessageType type, UnityEngine.Object context)
		{
			switch (type)
			{
				case MessageType.None:
				case MessageType.Info:
					Debug.Log(message, context);
					break;
				case MessageType.Warning:
					Debug.LogWarning(message, context);
					break;
				case MessageType.Error:
					Debug.LogError(message, context);
					break;
			}
		}
	}
}
