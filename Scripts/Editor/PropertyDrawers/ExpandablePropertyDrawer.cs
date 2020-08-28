using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(ExpandableAttribute))]
	public class ExpandablePropertyDrawer : PropertyDrawerBase
	{
		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			System.Type propertyType = PropertyUtility.GetPropertyType(property);
			if (typeof(ScriptableObject).IsAssignableFrom(propertyType))
			{
				ScriptableObject scriptableObject = property.objectReferenceValue as ScriptableObject;
				if (scriptableObject == null)
				{
					return GetPropertyHeight(property);
				}

				if (property.isExpanded)
				{
					using (SerializedObject serializedObject = new SerializedObject(scriptableObject))
					{
						float totalHeight = EditorGUIUtility.singleLineHeight;

						using (var iterator = serializedObject.GetIterator())
						{
							if (iterator.NextVisible(true))
							{
								do
								{
									SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
									if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
									{
										continue;
									}

									bool visible = PropertyUtility.IsVisible(childProperty);
									if (!visible)
									{
										continue;
									}

									float height = GetPropertyHeight(childProperty);
									totalHeight += height;
								}
								while (iterator.NextVisible(false));
							}
						}

						totalHeight += EditorGUIUtility.standardVerticalSpacing;
						return totalHeight;
					}
				}
				else
				{
					return GetPropertyHeight(property);
				}
			}
			else
			{
				return GetPropertyHeight(property) + GetHelpBoxHeight();
			}
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			EditorGUI.BeginProperty(rect, label, property);

			System.Type propertyType = PropertyUtility.GetPropertyType(property);
			if (typeof(ScriptableObject).IsAssignableFrom(propertyType))
			{
				ScriptableObject scriptableObject = property.objectReferenceValue as ScriptableObject;
				if (scriptableObject == null)
				{
					EditorGUI.PropertyField(rect, property, label, false);
				}
				else
				{
					// Draw a foldout
					Rect foldoutRect = new Rect()
					{
						x = rect.x,
						y = rect.y,
						width = EditorGUIUtility.labelWidth,
						height = EditorGUIUtility.singleLineHeight
					};

					property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, toggleOnLabelClick: true);

					// Draw the scriptable object field
					float indentLength = NaughtyEditorGUI.GetIndentLength(rect);
					float labelWidth = EditorGUIUtility.labelWidth - indentLength + NaughtyEditorGUI.HorizontalSpacing;
					Rect propertyRect = new Rect()
					{
						x = rect.x + labelWidth,
						y = rect.y,
						width = rect.width - labelWidth,
						height = EditorGUIUtility.singleLineHeight
					};

					EditorGUI.BeginChangeCheck();
					property.objectReferenceValue = EditorGUI.ObjectField(propertyRect, GUIContent.none, property.objectReferenceValue, propertyType, false);
					if (EditorGUI.EndChangeCheck())
					{
						property.serializedObject.ApplyModifiedProperties();
					}

					// Draw the child properties
					if (property.isExpanded)
					{
						DrawChildProperties(rect, property);
					}
				}
			}
			else
			{
				string message = $"{typeof(ExpandableAttribute).Name} can only be used on scriptable objects";
				DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
			}

			EditorGUI.EndProperty();
		}

		private void DrawChildProperties(Rect rect, SerializedProperty property)
		{
			ScriptableObject scriptableObject = property.objectReferenceValue as ScriptableObject;
			if (scriptableObject == null)
			{
				return;
			}

			Rect boxRect = new Rect()
			{
				x = 0.0f,
				y = rect.y + EditorGUIUtility.singleLineHeight,
				width = rect.width * 2.0f,
				height = rect.height - EditorGUIUtility.singleLineHeight
			};

			GUI.Box(boxRect, GUIContent.none);

			using (new EditorGUI.IndentLevelScope())
			{
				EditorGUI.BeginChangeCheck();

				SerializedObject serializedObject = new SerializedObject(scriptableObject);
				using (var iterator = serializedObject.GetIterator())
				{
					float yOffset = EditorGUIUtility.singleLineHeight;

					if (iterator.NextVisible(true))
					{
						do
						{
							SerializedProperty childProperty = serializedObject.FindProperty(iterator.name);
							if (childProperty.name.Equals("m_Script", System.StringComparison.Ordinal))
							{
								continue;
							}

							bool visible = PropertyUtility.IsVisible(childProperty);
							if (!visible)
							{
								continue;
							}

							float childHeight = GetPropertyHeight(childProperty);
							Rect childRect = new Rect()
							{
								x = rect.x,
								y = rect.y + yOffset,
								width = rect.width,
								height = childHeight
							};

							NaughtyEditorGUI.PropertyField(childRect, childProperty, true);

							yOffset += childHeight;
						}
						while (iterator.NextVisible(false));
					}
				}

				if (EditorGUI.EndChangeCheck())
				{
					serializedObject.ApplyModifiedProperties();
				}
			}
		}
	}
}
