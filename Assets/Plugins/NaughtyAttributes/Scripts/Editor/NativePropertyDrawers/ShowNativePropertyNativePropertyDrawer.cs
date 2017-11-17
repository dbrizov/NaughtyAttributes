using UnityEngine;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [NativePropertyDrawer(typeof(ShowNativePropertyAttribute))]
    public class ShowNativePropertyNativePropertyDrawer : NativePropertyDrawer
    {
        public override void DrawNativeProperty(UnityEngine.Object target, PropertyInfo property)
        {
            GUI.enabled = false;

            EditorDrawUtility.DrawLayoutField(property.GetValue(target, null), property.Name);

            GUI.enabled = true;
        }
    }
}
