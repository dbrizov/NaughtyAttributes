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
		protected List<NaughtyProperty> _serializedProperties = new List<NaughtyProperty>();
		protected List<FieldInfo> _nonSerializedFields;
		protected List<PropertyInfo> _nativeProperties;
		protected List<MethodInfo> _methods;

		protected List<NaughtyProperty> _nonGroupedSerializedProperty;
		protected SerializedProperty m_ScriptProperty;

		protected List<IGrouping<string, NaughtyProperty>> _groupedSerialzedProperty;
		protected List<IGrouping<string, NaughtyProperty>> _foldoutGroupedSerializedProperty;
		
		private Dictionary<string, SavedBool> _foldouts = new Dictionary<string, SavedBool>();

		private bool _anyNaughtyAttribute;
		
		protected bool _useCachedMetaAttributes;
		protected bool _changeDetected;
		
		protected virtual void OnEnable()
		{
			this.Prepare();
		}

		protected virtual void OnDisable()
		{
			//cleanup memory
			ReorderableListPropertyDrawer.Instance.ClearCache();

			_nonSerializedFields.Clear();
			_nativeProperties.Clear();
			_methods.Clear();
			_foldouts.Clear();

			_foldoutGroupedSerializedProperty.Clear();
			_groupedSerialzedProperty.Clear();
			_nonGroupedSerializedProperty.Clear();
			_serializedProperties.Clear();
			
			m_ScriptProperty = default;
		}

		public virtual void Prepare()
		{
			_nonSerializedFields = ReflectionUtility.GetAllFields(
				target, f => f.GetCustomAttributes(typeof(ShowNonSerializedFieldAttribute), true).Length > 0).ToList();

			_nativeProperties = ReflectionUtility.GetAllProperties(
				target, p => p.GetCustomAttributes(typeof(ShowNativePropertyAttribute), true).Length > 0).ToList();

			_methods = ReflectionUtility.GetAllMethods(
				target, m => m.GetCustomAttributes(typeof(ButtonAttribute), true).Length > 0).ToList();

			GetSerializedProperties(ref _serializedProperties);
			
			_anyNaughtyAttribute = _serializedProperties.Any(p => PropertyUtility.GetAttribute<INaughtyAttribute>(p.serializedProperty) != null);

			_nonGroupedSerializedProperty = GetNonGroupedProperties(_serializedProperties).ToList();
			
			//.First(...) doesnt work for some reason because the m_Script field isnt loaded yet I assume
			NaughtyProperty[] mScripts = _serializedProperties.Where(p => p.serializedProperty.name.Equals("m_Script")).ToArray();
			m_ScriptProperty = mScripts.Length > 0 ? mScripts[0].serializedProperty : null;
			
			_groupedSerialzedProperty = GetGroupedProperties(_serializedProperties).ToList();

			_foldoutGroupedSerializedProperty = GetFoldoutProperties(_serializedProperties).ToList();

			_useCachedMetaAttributes = false;
		}
		
		public override void OnInspectorGUI()
		{
			_changeDetected = false;
			
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

			_useCachedMetaAttributes = !_changeDetected;
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
				if (!_useCachedMetaAttributes)
				{
					naughtyProperty.cachedIsVisible = PropertyUtility.IsVisible(naughtyProperty.showIfAttribute,
						naughtyProperty.serializedProperty);
					
					naughtyProperty.cachedIsEnabled = PropertyUtility.IsEnabled(naughtyProperty.readOnlyAttribute, naughtyProperty.enableIfAttribute,
						naughtyProperty.serializedProperty);
				}
				
				_changeDetected |= NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, includeChildren: true);
			}

			// Draw grouped serialized properties
			foreach (var group in _groupedSerialzedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = 
					_useCachedMetaAttributes 
						? group.Where(p => p.cachedIsVisible)
						: group.Where(p =>
						{
							p.cachedIsEnabled = PropertyUtility.IsEnabled(p.readOnlyAttribute, p.enableIfAttribute,
								p.serializedProperty);
							
							return p.cachedIsVisible =
									PropertyUtility.IsVisible(p.showIfAttribute, p.serializedProperty);
						});
				
				if (!visibleProperties.Any())
				{
					continue;
				}
			
				NaughtyEditorGUI.BeginBoxGroup_Layout(group.Key);
				foreach (var naughtyProperty in visibleProperties)
				{
					_changeDetected |= NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, includeChildren: true);
				}
				NaughtyEditorGUI.EndBoxGroup_Layout();
			}
			
			// Draw foldout serialized properties
			foreach (var group in _foldoutGroupedSerializedProperty)
			{
				IEnumerable<NaughtyProperty> visibleProperties = 
					_useCachedMetaAttributes 
						? group.Where(p => p.cachedIsVisible)
						: group.Where(p =>
						{
							p.cachedIsEnabled = PropertyUtility.IsEnabled(p.readOnlyAttribute, p.enableIfAttribute,
								p.serializedProperty);
							
							return p.cachedIsVisible =
									PropertyUtility.IsVisible(p.showIfAttribute, p.serializedProperty);
						});
				
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
						_changeDetected |= NaughtyEditorGUI.PropertyField_Layout(naughtyProperty, true);
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
