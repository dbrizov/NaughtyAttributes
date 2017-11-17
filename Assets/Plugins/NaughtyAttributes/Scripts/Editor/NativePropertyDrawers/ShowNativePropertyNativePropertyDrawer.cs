using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : NativePropertyDrawer
    {
        public override void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property)
        {
            object value = property.GetValue(target, null);

            if (value == null)
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, "Reference");
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, property.Name))
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNativePropertyNativePropertyDrawer).Name, property.PropertyType.Name);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}
