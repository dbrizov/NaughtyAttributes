using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEditor;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
	[CanEditMultipleObjects]
	[CustomEditor(typeof(UnityEngine.Object), true)]
	public class NaughtyInspector : UnityEditor.Editor
	{
		private List<NaughtyProperty> _serializedProperties = new List<NaughtyProperty>();
		private IEnumerable<FieldInfo> _nonSerializedFields;
		private IEnumerable<PropertyInfo> _nativeProperties;
		private IEnumerable<MethodInfo> _methods;

		private IEnumerable<NaughtyProperty> _nonGroupedSerializedProperty;

		private SerializedProperty m_ScriptProperty;

		private IEnumerable<IGrouping<string, NaughtyProperty>> _groupedSerialzedProperty;

		private IEnumerable<IGrouping<string, NaughtyProperty>> _foldoutGroupedSerializedProperty;
		
		private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();

		private bool _anyNaughtyAttribute;
		
		protected virtual void OnEnable()
		{
			/*
			 * TODO:
			 * OnEnable is called for all monos and scriptable objects,
			 * which eats some one time perf after compilation and also takes some memory (although not noticeable)
			 * any other way to trigger this like via a custom editor/ window with on focus??
			 *
			 * Selection.selectionChanged += ????
			 * Base Mono and SO scripts that handle this??
			 */
			
			this.Prepare();
		}

		protected virtual void OnDisable()
		{
			//cleanup memory
			ReorderableListPropertyDrawer.Instance.ClearCache();

			_foldoutGroupedSerializedProperty = Enumerable.Empty<IGrouping<string, NaughtyProperty>>();
			_groupedSerialzedProperty = Enumerable.Empty<IGrouping<string, NaughtyProperty>>();
			_nonGroupedSerializedProperty = Enumerable.Empty<NaughtyProperty>();
			_serializedProperties.Clear();
			
			m_ScriptProperty = default;
		}

		public virtual void Prepare()
		{
			_nonSerializedFields = ReflectionUtility.GetAllFields(
				target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0);

			_nativeProperties = ReflectionUtility.GetAllProperties(
				target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0);

			_methods = ReflectionUtility.GetAllMethods(
				target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0);

			GetSerializedProperties(ref _serializedProperties);
			
			_anyNaughtyAttribute = _serializedProperties.Any(p => PropertyUtility.GetAttribute<INaughtyAttribute>(p.serializedProperty) != null);

			_nonGroupedSerializedProperty = GetNonGroupedProperties(_serializedProperties);
			
			//.First(...) doesnt work for some reason because the m_Script field isnt loaded yet I assume
			NaughtyProperty[] mScripts = _serializedProperties.Where(p => p.serializedProperty.name.Equals("m_Script")).ToArray();
			m_ScriptProperty = mScripts.Length > 0 ? mScripts[0].serializedProperty : null;
			
			_groupedSerialzedProperty = GetGroupedProperties(_serializedProperties);

			_foldoutGroupedSerializedProperty = GetFoldoutProperties(_serializedProperties);
		}
		
		public override void OnInspectorGUI()
		{
			if (!_anyNaughtyAttribute)
			{
				DrawDefaultInspector();
			}
			else
			{
				DrawSerializedProperties();
			}

			DrawNonSerializedFields();
			DrawNativeProperties();
			DrawButtons();
		}
		
		protected virtual void GetSerializedProperties(ref List<NaughtyProperty> outSerializedProperties)
		{
			outSerializedProperties.Clear();
			outSerializedProperties.TrimExcess();
			
			using (var iterator = serializedObject.GetIterator())
			{
				if (iterator.NextVisible(true))
				{
					do
					{
						outSerializedProperties.Add(
							PropertyUtility.CreateNaughtyProperty(
								serializedObject.FindProperty(iterator.name)));
					}
					while (iterator.NextVisible(false));
				}
			}
		}

		protected virtual void DrawSerializedProperties()
		{
			serializedObject.Update();

			if (m_ScriptProperty != null)
			{
				using (new EditorGUI.DisabledScope(disabled: true))
				{
					EditorGUILayout.PropertyField(m_ScriptProperty);
				}
			}

			// Draw non-grouped serialized properties
			foreach (var naughtyProperty in _nonGroupedSerializedProperty)
			{
				NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, includeChildren: true);
			}

			// Draw grouped serialized properties
			foreach (var group in _groupedSerialzedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p.showIfAttribute, p.serializedProperty));
				if (!visibleProperties.Any())
				{
					continue;
				}
			
				NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
				foreach (var naughtyProperty in visibleProperties)
				{
					NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, includeChildren: true);
				}
			
				NaughtyEditorGUI.EndBoxGroup_Layout();
			}
			
			// Draw foldout serialized properties
			foreach (var group in _foldoutGroupedSerializedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = group.Where(p => PropertyUtility.IsVisible(p.showIfAttribute, p.serializedProperty));
				if (!visibleProperties.Any())
				{
					continue;
				}
			
				if (!_foldouts.ContainsKey(group.Key))
				{
					_foldouts[group.Key] = new SavedBool($"{target.GetInstanceID()}.{group.Key}", false);
				}
			
				_foldouts[group.Key].Value = EditorGUILayout.Foldout(_foldouts[group.Key].Value, group.Key, true);
				if (_foldouts[group.Key].Value)
				{
					foreach (var naughtyProperty in visibleProperties)
					{
						NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, true);
					}
				}
			}

			serializedObject.ApplyModifiedProperties();
		}

		protected virtual void DrawNonSerializedFields(bool drawHeader = false)
		{
			if (_nonSerializedFields.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Non-Serialized Fields", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var field in _nonSerializedFields)
				{
					NaughtyEditorGUI.NonSerializedField_Layout(serializedObject.targetObject, field);
				}
			}
		}

		protected virtual void DrawNativeProperties(bool drawHeader = false)
		{
			if (_nativeProperties.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Native Properties", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var property in _nativeProperties)
				{
					NaughtyEditorGUI.NativeProperty_Layout(serializedObject.targetObject, property);
				}
			}
		}

		protected virtual void DrawButtons(bool drawHeader = false)
		{
			if (_methods.Any())
			{
				if (drawHeader)
				{
					EditorGUILayout.Space();
					EditorGUILayout.LabelField("Buttons", GetHeaderGUIStyle());
					NaughtyEditorGUI.HorizontalLine(
						EditorGUILayout.GetControlRect(false), HorizontalLineAttribute.DefaultHeight, HorizontalLineAttribute.DefaultColor.GetColor());
				}

				foreach (var method in _methods)
				{
					NaughtyEditorGUI.Button(serializedObject.targetObject, method);
				}
			}
		}

		private static IEnumerable<NaughtyProperty> GetNonGroupedProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties.Where(p => PropertyUtility.GetAttribute<IGroupAttribute>(p.serializedProperty) == null && !p.serializedProperty.name.Equals("m_Script"));
		}

		private static IEnumerable<IGrouping<string, NaughtyProperty>> GetGroupedProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p.serializedProperty) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<BoxGroupAttribute>(p.serializedProperty).Name);
		}

		private static IEnumerable<IGrouping<string, NaughtyProperty>> GetFoldoutProperties(IEnumerable<NaughtyProperty> properties)
		{
			return properties
				.Where(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p.serializedProperty) != null)
				.GroupBy(p => PropertyUtility.GetAttribute<FoldoutAttribute>(p.serializedProperty).Name);
		}

		private static GUIStyle GetHeaderGUIStyle()
		{
			GUIStyle style = new GUIStyle(EditorStyles.centeredGreyMiniLabel);
			style.fontStyle = FontStyle.Bold;
			style.alignment = TextAnchor.UpperCenter;

			return style;
		}
	}
}
