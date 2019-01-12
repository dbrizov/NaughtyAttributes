using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyMeta(typeof(InfoBoxAttribute))]
    public class InfoBoxPropertyMeta : PropertyMeta
    {
        public override void ApplyPropertyMeta(SerializedProperty property, MetaAttribute metaAttribute)
        {
            InfoBoxAttribute infoBoxAttribute = (InfoBoxAttribute)metaAttribute;
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            if (!string.IsNullOrEmpty(infoBoxAttribute.VisibleIf))
            {
                FieldInfo conditionField = ReflectionUtility.GetField(target, infoBoxAttribute.VisibleIf);
                if (conditionField != null &&
                    conditionField.FieldType == typeof(bool))
                {
                    if ((bool)conditionField.GetValue(target))
                    {
                        this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
                    }

                    return;
                }

                MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, infoBoxAttribute.VisibleIf);
                if (conditionMethod != null &&
                    conditionMethod.ReturnType == typeof(bool) &&
                    conditionMethod.GetParameters().Length == 0)
                {
                    if ((bool)conditionMethod.Invoke(target, null))
                    {
                        this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
                    }

                    return;
                }

                string warning = infoBoxAttribute.GetType().Name + " needs a valid boolean condition field or method name to work";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: PropertyUtility.GetTargetObject(property));
            }
            else
            {
                this.DrawInfoBox(infoBoxAttribute.Text, infoBoxAttribute.Type);
            }
        }

        private void DrawInfoBox(string infoText, InfoBoxType infoBoxType)
        {
            switch (infoBoxType)
            {
                case InfoBoxType.Normal:
                    EditorGUILayout.HelpBox(infoText, MessageType.Info);
                    break;

                case InfoBoxType.Warning:
                    EditorGUILayout.HelpBox(infoText, MessageType.Warning);
                    break;

                case InfoBoxType.Error:
                    EditorGUILayout.HelpBox(infoText, MessageType.Error);
                    break;
            }
        }
    }
}
