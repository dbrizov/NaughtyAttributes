# Package Starter Kit

NaughtyAttributes is an extension for the Unity Inspector.

It expands the range of attributes that Unity provides so that you can create powerful inspectors without the need of
custom editors or property drawers. It also provides attributes that can be applied to non-serialized fields or
functions.

Most of the attributes are implemented using Unity's `CustomPropertyDrawer`, so they will work in your custom editors.
If you want all of the attributes to work in your custom editors, however, you must inherit from `NaughtyInspector` and
use the `NaughtyEditorGUI.PropertyField_Layout` function instead of `EditorGUILayout.PropertyField`.