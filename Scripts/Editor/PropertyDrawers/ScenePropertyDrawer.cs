using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Text.RegularExpressions;
using System;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(SceneAttribute))]
	public class ScenePropertyDrawer : PropertyDrawerBase
	{
		private const string SceneListItem = "{0} ({1})";
		private const string ScenePattern = @".+\/(.+)\.unity";
		private const string TypeWarningMessage = "{0} must be an int or a string";
		private const string BuildSettingsWarningMessage = "No scenes in the build settings";

		protected override float GetPropertyHeight_Internal(SerializedProperty property, GUIContent label)
		{
			bool validPropertyType = property.propertyType == SerializedPropertyType.String || property.propertyType == SerializedPropertyType.Integer;
			bool anySceneInBuildSettings = GetScenes().Length > 0;

			return (validPropertyType && anySceneInBuildSettings)
				? GetPropertyHeight(property)
				: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			string[] scenes = GetScenes();
			bool anySceneInBuildSettings = scenes.Length > 0;
			if (!anySceneInBuildSettings)
			{
				DrawDefaultPropertyAndHelpBox(rect, property, BuildSettingsWarningMessage, MessageType.Warning);
				return;
			}

			string[] sceneOptions = GetSceneOptions(scenes);
			switch (property.propertyType)
			{
				case SerializedPropertyType.String:
					DrawPropertyForString(rect, property, label, scenes, sceneOptions);
					break;
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label, sceneOptions);
					break;
				default:
					string message = string.Format(TypeWarningMessage, property.name);
					DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
					break;
			}
		}

		private string[] GetScenes()
		{
			return EditorBuildSettings.scenes
				.Where(scene => scene.enabled)
				.Select(scene => Regex.Match(scene.path, ScenePattern).Groups[1].Value)
				.ToArray();
		}

		private string[] GetSceneOptions(string[] scenes)
		{
			return scenes.Select((s, i) => string.Format(SceneListItem, s, i)).ToArray();
		}

		private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label, string[] scenes, string[] sceneOptions)
		{
			var index = IndexOf(scenes, property.stringValue);
			var newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);
			property.stringValue = scenes[newIndex];
		}

		private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label, string[] sceneOptions)
		{
			var index = property.intValue;
			var newIndex = EditorGUI.Popup(rect, label.text, index, sceneOptions);
			property.intValue = newIndex;
		}

		private static int IndexOf(string[] scenes, string scene)
		{
			var index = Array.IndexOf(scenes, scene);
			return Mathf.Clamp(index, 0, scenes.Length - 1);
		}
	}
}
