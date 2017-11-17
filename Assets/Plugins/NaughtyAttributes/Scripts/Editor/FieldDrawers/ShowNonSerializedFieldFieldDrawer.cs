using UnityEngine;
using System.Reflection;

namespace NaughtyAttributes.Editor
{
    [FieldDrawer(typeof(ShowNonSerializedFieldAttribute))]
    public class ShowNonSerializedFieldFieldDrawer : FieldDrawer
    {
        public override void DrawField(UnityEngine.Object target, FieldInfo field)
        {
            GUI.enabled = false;

            EditorDrawUtility.DrawLayoutField(field.GetValue(target), field.Name);

            GUI.enabled = true;
        }
    }
}
