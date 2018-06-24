using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfPropertyDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty property)
        {
        	bool drawEnabled = false;

			EnableIfAttribute disableIfAttribute = PropertyUtility.GetAttribute<EnableIfAttribute>(property);
			UnityEngine.Object target = PropertyUtility.GetTargetObject(property);

			FieldInfo conditionField = ReflectionUtility.GetField(target, disableIfAttribute.ConditionName);
            if (conditionField != null &&
                conditionField.FieldType == typeof(bool))
            {
				drawEnabled = (bool)conditionField.GetValue(target);
            }

			MethodInfo conditionMethod = ReflectionUtility.GetMethod(target, disableIfAttribute.ConditionName);
            if (conditionMethod != null &&
                conditionMethod.ReturnType == typeof(bool) &&
                conditionMethod.GetParameters().Length == 0)
            {
				drawEnabled = (bool)conditionMethod.Invoke(target, null);
            }

			GUI.enabled = drawEnabled;
            EditorDrawUtility.DrawPropertyField(property);
            GUI.enabled = true;
        }
    }
}
