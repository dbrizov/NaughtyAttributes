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
		private const string TypeWarningMessage = "{0} must be an int or a string.";
		private const string BuildSettingsWarningMessage = "{0}: A scene must be added and enabled in build settings.";

		private static string[] _scenes;
		private static string[] _sceneOptions;

		private bool _initialized;

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return
				property.propertyType == SerializedPropertyType.String ||
				property.propertyType == SerializedPropertyType.Integer
					? GetPropertyHeight(property)
					: GetPropertyHeight(property) + GetHelpBoxHeight();
		}

		protected override void OnGUI_Internal(Rect rect, SerializedProperty property, GUIContent label)
		{
			Initialize();

			switch (property.propertyType)
			{
				case SerializedPropertyType.String:
					DrawPropertyForString(rect, property, label);
					break;
				case SerializedPropertyType.Integer:
					DrawPropertyForInt(rect, property, label);
					break;
				default:
					string message = string.Format(TypeWarningMessage, property.name);
					DrawDefaultPropertyAndHelpBox(rect, property, message, MessageType.Warning);
					break;
			}
		}

		private void Initialize()
		{
			if (_initialized)
			{
				return;
			}

			OnSceneListChanged();
			EditorBuildSettings.sceneListChanged += OnSceneListChanged;
			_initialized = true;
		}

		private void OnSceneListChanged()
		{
			_scenes = EditorBuildSettings.scenes
				.Where(scene => scene.enabled)
				.Select(scene => Regex.Match(scene.path, ScenePattern).Groups[1].Value)
				.ToArray();

			_sceneOptions = _scenes.Select((s, i) => string.Format(SceneListItem, s, i)).ToArray();
		}

		private static void DrawPropertyForString(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (!ScenesExistInBuildSettings(rect, property))
			{
				return;
			}

			var index = IndexOf(property.stringValue);
			var newIndex = EditorGUI.Popup(rect, label.text, index, _sceneOptions);
			property.stringValue = _scenes[newIndex];
		}

		private static void DrawPropertyForInt(Rect rect, SerializedProperty property, GUIContent label)
		{
			if (!ScenesExistInBuildSettings(rect, property))
			{
				return;
			}

			var index = property.intValue;
			var newIndex = EditorGUI.Popup(rect, label.text, index, _sceneOptions);
			property.intValue = newIndex;
		}

		private static int IndexOf(string scene)
		{
			var index = Array.IndexOf(_scenes, scene);
			return Mathf.Clamp(index, 0, _scenes.Length - 1);
		}

		private static bool ScenesExistInBuildSettings(Rect rect, SerializedProperty property)
		{
			if (_scenes.Length != 0)
			{
				return true;
			}

			NaughtyEditorGUI.HelpBox(rect, string.Format(BuildSettingsWarningMessage, property.name), MessageType.Warning);
			return false;
		}
	}
}
