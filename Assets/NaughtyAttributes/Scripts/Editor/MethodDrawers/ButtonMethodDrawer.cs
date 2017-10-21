using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(System.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.ReturnType == typeof(void) &&
                methodInfo.GetParameters().Length == 0)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                if (GUILayout.Button(buttonText))
                {
                    methodInfo.Invoke(target, null);
                }
            }
            else
            {
                string warning = "ButtonAttribute works only on action methods - with void return type and no parameters";
                EditorGUILayout.HelpBox(warning, MessageType.Warning);
                Debug.LogWarning(warning);
            }
        }
    }
}
