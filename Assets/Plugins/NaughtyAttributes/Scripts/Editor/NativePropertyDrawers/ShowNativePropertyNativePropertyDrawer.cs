using UnityEngine;
using System.Reflection;
using System.Collections;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : NativePropertyDrawer
    {
        public override void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property)
        {
            bool isPropertyACollection =
                !typeof(string).Equals(property.PropertyType) &&
                typeof(IEnumerable).IsAssignableFrom(property.PropertyType);

            if (!isPropertyACollection)
            {
                GUI.enabled = false;
                EditorDrawUtility.DrawLayoutField(property.GetValue(target, null), property.Name);
                GUI.enabled = true;
            }
            else
            {
                string warning = "Can't draw collection types";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}
