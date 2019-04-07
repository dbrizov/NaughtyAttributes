using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Experimental.SceneManagement;
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

                if (GUILayout.Button(buttonText))
                {
                    methodInfo.Invoke(target, null);
                    
                    // set prefab scene as dirty to flush changes in prefab mode
                    var stage = PrefabStageUtility.GetCurrentPrefabStage();
                    if (stage != null) {
                        EditorSceneManager.MarkSceneDirty(stage.scene);
                    }
                }
            }
            else
            {
                string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
                EditorDrawUtility.DrawHelpBox(warning, MessageType.Warning, context: target);
            }
        }
    }
}
