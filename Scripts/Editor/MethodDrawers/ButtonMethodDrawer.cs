using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	public class ButtonMethodDrawer
	{
		public void DrawMethod(UnityEngine.Object target, MethodInfo methodInfo)
		{
			if (methodInfo.GetParameters().Length == 0)
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
				string warning = typeof(ButtonAttribute).Name + " works only on methods with no parameters";
				EditorGUIExtensions.HelpBox_Layout(warning, MessageType.Warning, context: target);
			}
		}
	}
}
