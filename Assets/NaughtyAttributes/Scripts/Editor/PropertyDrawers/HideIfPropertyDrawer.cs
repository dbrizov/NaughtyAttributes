using System.Reflection;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
            HideIfAttribute hideIfAttribute = PropertyUtility.GetAttributes<HideIfAttribute>(property)[0];
            UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

            FieldInfo conditionField = target.GetType().GetField(hideIfAttribute.ConditionName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionField != null)
            {
                if (!(bool)conditionField.GetValue(target))
                {
                    EditorGUILayout.PropertyField(property);
                }

                return;
            }

            MethodInfo conditionMethod = target.GetType().GetMethod(hideIfAttribute.ConditionName, BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
                if (!(bool)conditionMethod.Invoke(target, null))
                {
                    EditorGUILayout.PropertyField(property);
                }

                return;
            }

            EditorGUILayout.HelpBox(hideIfAttribute.GetType().Name + " needs a valid condition field or method name to work", MessageType.Warning);
            EditorGUILayout.PropertyField(property);
        }
    }
}
