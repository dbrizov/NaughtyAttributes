using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
	public class ReorderableListPropertyDrawer : SpecialCasePropertyDrawerBase
	{
		public static readonly ReorderableListPropertyDrawer Instance = new ReorderableListPropertyDrawer();

		private readonly Dictionary<string, ReorderableList> _reorderableListsByPropertyName = new Dictionary<string, ReorderableList>();

		private string GetPropertyKeyName(SerializedProperty property)
		{
			return property.serializedObject.targetObject.GetInstanceID() + "/" + property.name;
		}

		protected override void OnGUI_Internal(SerializedProperty property, GUIContent label)
		{
			if (property.isArray)
			{
				string key = GetPropertyKeyName(property);

				if (!_reorderableListsByPropertyName.ContainsKey(key))
				{
					ReorderableList reorderableList = null;
					reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
					{
						drawHeaderCallback = (Rect rect) =>
						{
							EditorGUI.LabelField(rect, string.Format("{0}: {1}", label.text, property.arraySize), EditorStyles.boldLabel);
							HandleDragAndDrop(rect, reorderableList);
						},

						drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
						{
							SerializedProperty element = property.GetArrayElementAtIndex(index);
							rect.y += 1.0f;
							rect.x += 10.0f;
							rect.width -= 10.0f;

							EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index))), element, true);
						},

						elementHeightCallback = (int index) =>
						{
							return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index)) + 4.0f;
						}
					};

					_reorderableListsByPropertyName[key] = reorderableList;
				}

				_reorderableListsByPropertyName[key].DoLayoutList();
			}
			else
			{
				string message = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
				NaughtyEditorGUI.HelpBox_Layout(message, MessageType.Warning, context: property.serializedObject.targetObject);
				EditorGUILayout.PropertyField(property, true);
			}
		}
		private bool IsAssignable(Object obj, ReorderableList list)
		{
			System.Type type = ReflectionUtility.GetType(list.serializedProperty);
			type = ReflectionUtility.IfListGetInnerTypeOfList(type);
			return type.IsAssignableFrom(obj.GetType());
		}

		private void HandleDragAndDrop(Rect rect, ReorderableList list)
		{
			var currentEvent = Event.current;
			var usedEvent = false;
			;
			switch (currentEvent.type)
			{
				case EventType.DragExited:
					if (GUI.enabled)
						HandleUtility.Repaint();
					break;

				case EventType.DragUpdated:
				case EventType.DragPerform:
					if (rect.Contains(currentEvent.mousePosition) && GUI.enabled)
					{
						// Check each single object, so we can add multiple objects in a single drag.
						bool didAcceptDrag = false;
						Object[] references = DragAndDrop.objectReferences;
						foreach (Object obj in references)
						{
							if (IsAssignable(obj, list))
							{
								DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
								if (currentEvent.type == EventType.DragPerform)
								{
									list.serializedProperty.arraySize++;
									int arrayEnd = list.serializedProperty.arraySize - 1;
									list.serializedProperty.GetArrayElementAtIndex(arrayEnd).objectReferenceValue = obj;
									didAcceptDrag = true;
								}
							}
						}
						if (didAcceptDrag)
						{
							GUI.changed = true;
							DragAndDrop.AcceptDrag();
							usedEvent = true;
						}
					}
					break;
				case EventType.ValidateCommand:
				case EventType.ExecuteCommand:
					break;
			}
			if (usedEvent)
			{
				currentEvent.Use();
			}
		}

		public void ClearCache()
		{
			_reorderableListsByPropertyName.Clear();
		}
	}
}
