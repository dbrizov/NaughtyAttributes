using System;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(SceneAttribute))]
    public class ScenePropertyDrawer : PropertyDrawer
    {
        private const string SceneListItem = "{0} ({1})";
        private const string ScenePattern = @".+\/(.+)\.unity";
        private const string TypeWarningMessage = "{0} must be an int or a string.";
        private const string BuildSettingsWarningMessage = "{0}: A scene must be added and enabled in build settings.";

        private static string[] _scenes;
        private static string[] _sceneOptions;

        private bool _initialized;

        public override void DrawProperty(SerializedProperty property)
        {
            Initialize();

            switch (property.propertyType)
            {
                case SerializedPropertyType.String:
                    DrawPropertyForString(property);
                    break;
                case SerializedPropertyType.Integer:
                    DrawPropertyForInt(property);
                    break;
                default:
                    EditorGUILayout.HelpBox(string.Format(TypeWarningMessage, property.name), MessageType.Warning);
                    break;
            }
        }

        private void Initialize()
        {
            if (_initialized) return;

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

        private static void DrawPropertyForString(SerializedProperty property)
        {
            if (!ScenesExistInBuildSettings(property)) return;

            var index = IndexOf(property.stringValue);
            var newIndex = EditorGUILayout.Popup(property.displayName, index, _sceneOptions);
            property.stringValue = _scenes[newIndex];
        }

        private static void DrawPropertyForInt(SerializedProperty property)
        {
            if (!ScenesExistInBuildSettings(property)) return;

            var index = property.intValue;
            var newIndex = EditorGUILayout.Popup(property.displayName, index, _sceneOptions);
            property.intValue = newIndex;
        }

        private static int IndexOf(string scene)
        {
            var index = Array.IndexOf(_scenes, scene);
            return Mathf.Clamp(index, 0, _scenes.Length - 1);
        }

        private static bool ScenesExistInBuildSettings(SerializedProperty property)
        {
            if (_scenes.Length != 0) return true;

            EditorGUILayout.HelpBox(string.Format(BuildSettingsWarningMessage, property.name), MessageType.Warning);
            return false;
        }
    }
}
