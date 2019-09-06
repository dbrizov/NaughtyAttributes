using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [MethodDrawer(typeof(ButtonAttribute))]
    public class ButtonMethodDrawer : MethodDrawer
    {
        public override void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
        {
            if (methodInfo.GetParameters().Length == 0)
            {
                ButtonAttribute buttonAttribute = (ButtonAttribute)methodInfo.GetCustomAttributes(typeof(ButtonAttribute), true)[0];
                string buttonText = string.IsNullOrEmpty(buttonAttribute.Text) ? methodInfo.Name : buttonAttribute.Text;

                // Check are we selecting im project folder or in hierarchy window.
                var IsAssetSelection = Selection.activeTransform == null;
                var activeInPlayMode = EditorApplication.isPlaying && !buttonAttribute.activeInPlayMode;
                var activeInEditMode = !EditorApplication.isPlaying && !buttonAttribute.activeInEditMode;

                var enabled = activeInPlayMode ? false : activeInEditMode ? false : true;

                if (enabled)
                    if (!activeInEditMode && IsAssetSelection) // Disable button if we select an asset (like prefab). It does not matter if we are in playMode.
                        enabled = false;

                GUI.enabled = enabled;
                if (GUILayout.Button(buttonText))
                {
                    methodInfo.Invoke(target, null);
                }
                GUI.enabled = true;
            }
            else
            {
                string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
