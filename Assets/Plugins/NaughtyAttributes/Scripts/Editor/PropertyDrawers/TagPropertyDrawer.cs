using UnityEditor;
using System.Collections.Generic;

namespace NaughtyAttributes.Editor
{
    // Original by Dylan Engelman 
    // http://jupiterlighthousestudio.com/custom-inspectors-unity/
    // Altered by Brecht Lecluyse http://www.brechtos.com 
    // and Sichen Liu https://sichenn.github.io
    [PropertyDrawer(typeof(TagAttribute))]
    public class TagPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                // generate the taglist + custom tags
                List<string> tagList = new List<string>();
                tagList.Add("(None)");
                tagList.Add("Untagged");
                tagList.AddRange(UnityEditorInternal.InternalEditorUtility.tags);

                string propertyString = property.stringValue;
                int index = 0;
                // check if there is an entry that matches the entry and get the index
                // we skip index 0 as that is a special custom case
                for (int i = 1; i < tagList.Count; i++)
                {
                    if (tagList[i] == propertyString)
                    {
                        index = i;
                        break;
                    }
                }

                // Draw the popup box with the current selected index
                index = EditorGUILayout.Popup(property.displayName, index, tagList.ToArray());

                // Adjust the actual string value of the property based on the selection
                if (index > 0)
                {
                    property.stringValue = tagList[index];
                }
                else
                {
                    property.stringValue = string.Empty;
                }
            }
            else
            {
                EditorGUILayout.HelpBox(property.type + " is not supported by TagAttribute\n" +
                "Use string instead", MessageType.Warning);
            }
        }
    }
}