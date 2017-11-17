using UnityEngine;
using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [FieldDrawer(typeof(ShowNonSerializedFieldAttribute))]
    public class ShowNonSerializedFieldFieldDrawer : FieldDrawer
    {
        public override void DrawField(UnityEngine.Object target, FieldInfo field)
        {
            object value = field.GetValue(target);

            if (value == null)
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldFieldDrawer).Name, "Reference");
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
            else if (!EditorDrawUtility.DrawLayoutField(value, field.Name))
            {
                string warning = string.Format("{0} doesn't support {1} types", typeof(ShowNonSerializedFieldFieldDrawer).Name, field.FieldType.Name);
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning, target);
            }
        }
    }
}
