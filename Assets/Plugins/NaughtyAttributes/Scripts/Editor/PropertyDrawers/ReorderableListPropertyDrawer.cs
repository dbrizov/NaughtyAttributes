using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(ReorderableListAttribute))]
    public class ReorderableListPropertyDrawer : PropertyDrawer
    {
        private Dictionary<string, ReorderableList> reorderableListsByPropertyName = new Dictionary<string, ReorderableList>();
        public int currentPage = 0;
        public int totalPages;

        public override void DrawProperty(SerializedProperty property)
        {
            EditorDrawUtility.DrawHeader(property);
            ReorderableListAttribute attrib = PropertyUtility.GetAttribute<ReorderableListAttribute>(property);
            totalPages = Mathf.CeilToInt((float)property.arraySize / attrib.pageSize);
            if (property.isArray)
            {
                if (!this.reorderableListsByPropertyName.ContainsKey(property.name))
                {
                    ReorderableList reorderableList = new ReorderableList(property.serializedObject, property, true, true, true, true)
                    {
                        drawHeaderCallback = (Rect rect) => 
                        {
                            EditorGUI.LabelField(rect, string.Format("{0}: {1}", property.displayName, property.arraySize), EditorStyles.label);
                        },

                        drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
                        {
                            int minElement = 0;
                            int maxElement = property.arraySize;
                            if (attrib.paginate)
                            {
                                minElement = currentPage * attrib.pageSize;
                                maxElement = minElement + attrib.pageSize;
                            }
                            if(index >= minElement && index < maxElement)
                            {
                                var element = property.GetArrayElementAtIndex(index);
                                rect.y += 2f;
                                rect.x += 10f;
                                rect.width -= 10f;

                                EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 0), element, true);
                            }
                        },
                        elementHeightCallback = index =>
                        {
                            int minElement = 0;
                            int maxElement = property.arraySize;
                            if (attrib.paginate)
                            {
                                minElement = currentPage * attrib.pageSize;
                                maxElement = minElement + attrib.pageSize;
                            }
                            if (index >= minElement && index < maxElement)
                            {
                                return EditorGUI.GetPropertyHeight(property.GetArrayElementAtIndex(index));
                            }
                            else
                            {
                                return 0f;
                            }
                        }
                    };
                    if(attrib.paginate)
                    {
                        reorderableList.drawFooterCallback = (Rect rect) =>
                        {
                            if (Event.current.type == EventType.Repaint)
                            {
                                ReorderableList.defaultBehaviours.footerBackground.Draw(rect, false, false, false, false);
                            }
                            Rect addRect = new Rect(rect.xMin + 4f, rect.y - 3f, 25f, 13f);
                            Rect subRect = new Rect(rect.xMax - 29f, rect.y - 3f, 25f, 13f);
                            if (GUI.Button(addRect, EditorGUIUtility.IconContent("Animation.PrevKey", "Previous page"), new GUIStyle("RL FooterButton")))
                            {
                                if (currentPage > 0)
                                {
                                    currentPage--;
                                }
                                else
                                {
                                    currentPage = 0;
                                }
                            }
                            if (GUI.Button(subRect, EditorGUIUtility.IconContent("Animation.NextKey", "Next page"), ReorderableList.defaultBehaviours.preButton))
                            {
                                if (currentPage < totalPages - 1)
                                {
                                    currentPage++;
                                }
                                else
                                {
                                    currentPage = totalPages - 1;
                                }
                            }
                            EditorGUI.LabelField(rect, string.Format("{0}/{1}", currentPage + 1, totalPages), new GUIStyle(EditorStyles.label)
                            {
                                alignment = TextAnchor.MiddleCenter
                            });
                        };
                    }
                    this.reorderableListsByPropertyName[property.name] = reorderableList;
                }
                this.reorderableListsByPropertyName[property.name].DoLayoutList();
            }
            else
            {
                string warning = typeof(ReorderableListAttribute).Name + " can be used only on arrays or lists";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, PropertyUtility.GetTargetObject(property));
                
                EditorDrawUtility.DrawPropertyField(property);
            }
        }

        public override void ClearCache()
        {
            this.reorderableListsByPropertyName.Clear();
        }
    }
}
