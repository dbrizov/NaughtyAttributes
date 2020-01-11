using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(UsePropertySetterAttribute))]
    public class UsePropertySetterDrawer : PropertyDrawer
    {
        public override void DrawProperty(SerializedProperty serializedProperty)
        {
            string warningMessage = "";

            //Find the property member
            PropertyInfo property = FindProperty(serializedProperty);
            if (property == null || property.SetMethod == null)
            {
                EditorDrawUtility.DrawHelpBox($"No setter was found for member {serializedProperty.name}.",
                    MessageType.Error, logToConsole: false, context: serializedProperty.serializedObject.context);
                return;
            }

            //Draw the appropiate field and get the value entered by the user in the inspector.
            bool valueChanged = DrawControl(serializedProperty, out object valueSetInInspector, ref warningMessage);

            if (valueChanged)
            {
                //Support undo.
                //TODO: As of right now, the SerializedProperty's value change is correctly undone, but the setter doesn't get called with the old value.
                //I'm not sure if that's possible, or even if it would be better that way.
                Undo.RecordObject(serializedProperty.serializedObject.targetObject, $"Changed {serializedProperty.displayName} in inspector.");
                
                //Call the setter
                property.SetValue(serializedProperty.serializedObject.targetObject, valueSetInInspector);

                //Read the value after the setter
                object processedValue = FindField(serializedProperty).GetValue(serializedProperty.serializedObject.targetObject);

                //Set the SerializedProperty value to the read value
                ModifySerializedValue(serializedProperty, processedValue, ref warningMessage);
            }

            //If we have a warning message, show it.
            if (!string.IsNullOrEmpty(warningMessage))
            {
                warningMessage = warningMessage.Trim();
                EditorDrawUtility.DrawHelpBox(warningMessage, MessageType.Warning,
                    logToConsole: false, context: serializedProperty.serializedObject.targetObject);
            }
        }


        /// <summary>
        /// Draws the appropiate control for this serialized property, returns wether the value changed and gives out the new value.
        /// </summary>
        private bool DrawControl(SerializedProperty serializedProperty, out object newValue, ref string warningMessage)
        {
            EditorGUI.BeginChangeCheck();

            var guiContent = new GUIContent(serializedProperty.displayName, GetPropertyTooltip(serializedProperty));
            EditorGUILayout.PropertyField(serializedProperty, guiContent, true);
            newValue = ReadSerializedValue(serializedProperty, ref warningMessage);

            return EditorGUI.EndChangeCheck();
        } 

        private object ReadSerializedValue(SerializedProperty serializedProperty, ref string warningMessage)
        {
            object oldValue = null;
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    oldValue = serializedProperty.intValue;
                    break;
                case SerializedPropertyType.Boolean:
                    oldValue = serializedProperty.boolValue;
                    break;
                case SerializedPropertyType.Float:
                    oldValue = serializedProperty.floatValue;
                    break;
                case SerializedPropertyType.String:
                    oldValue = serializedProperty.stringValue;
                    break;
                case SerializedPropertyType.Color:
                    oldValue = serializedProperty.colorValue;
                    break;
                case SerializedPropertyType.ObjectReference:
                    oldValue = serializedProperty.objectReferenceValue;
                    break;
                case SerializedPropertyType.LayerMask:
                    oldValue = serializedProperty.intValue;
                    break;
                case SerializedPropertyType.Enum:
                    oldValue = serializedProperty.enumValueIndex;
                    break;
                case SerializedPropertyType.Vector2:
                    oldValue = serializedProperty.vector2Value;
                    break;
                case SerializedPropertyType.Vector3:
                    oldValue = serializedProperty.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    oldValue = serializedProperty.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    oldValue = serializedProperty.rectValue;
                    break;
                case SerializedPropertyType.ArraySize:
                    oldValue = serializedProperty.arraySize;
                    break;
                case SerializedPropertyType.Character:
                    oldValue = serializedProperty.stringValue;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    oldValue = serializedProperty.animationCurveValue;
                    break;
                case SerializedPropertyType.Bounds:
                    oldValue = serializedProperty.boundsValue;
                    break;
                case SerializedPropertyType.Quaternion:
                    oldValue = serializedProperty.quaternionValue;
                    break;
                case SerializedPropertyType.ExposedReference:
                    oldValue = serializedProperty.exposedReferenceValue;
                    break;
                case SerializedPropertyType.FixedBufferSize:
                    oldValue = serializedProperty.fixedBufferSize;
                    break;
                case SerializedPropertyType.Vector2Int:
                    oldValue = serializedProperty.vector2IntValue;
                    break;
                case SerializedPropertyType.Vector3Int:
                    oldValue = serializedProperty.vector3IntValue;
                    break;
                case SerializedPropertyType.RectInt:
                    oldValue = serializedProperty.rectIntValue;
                    break;
                case SerializedPropertyType.BoundsInt:
                    oldValue = serializedProperty.boundsIntValue;
                    break;
                default:
                    if (string.IsNullOrEmpty(warningMessage))
                        warningMessage += $"{serializedProperty.propertyType} serialized types aren't supported by UsePropertySetterAttribute.";
                    
                    break;
            }

            return oldValue;
        }

        private void ModifySerializedValue(SerializedProperty serializedProperty, object value, ref string warningMessage)
        {
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    serializedProperty.intValue = (int)value;
                    break;
                case SerializedPropertyType.Boolean:
                    serializedProperty.boolValue = (bool)value;
                    break;
                case SerializedPropertyType.Float:
                    serializedProperty.floatValue = (float)value;
                    break;
                case SerializedPropertyType.String:
                    serializedProperty.stringValue = (string)value;
                    break;
                case SerializedPropertyType.Color:
                    serializedProperty.colorValue = (Color)value;
                    break;
                case SerializedPropertyType.ObjectReference:
                    serializedProperty.objectReferenceValue = value as Object;
                    break;
                case SerializedPropertyType.LayerMask:
                    serializedProperty.intValue = (LayerMask)value;
                    break;
                case SerializedPropertyType.Enum:
                    serializedProperty.enumValueIndex = (int)value;
                    break;
                case SerializedPropertyType.Vector2:
                    serializedProperty.vector2Value = (Vector2)value;
                    break;
                case SerializedPropertyType.Vector3:
                    serializedProperty.vector3Value = (Vector3)value;
                    break;
                case SerializedPropertyType.Vector4:
                    serializedProperty.vector4Value = (Vector4)value;
                    break;
                case SerializedPropertyType.Rect:
                    serializedProperty.rectValue = (Rect)value;
                    break;
                case SerializedPropertyType.AnimationCurve:
                    serializedProperty.animationCurveValue = (AnimationCurve)value;
                    break;
                case SerializedPropertyType.Bounds:
                    serializedProperty.boundsValue = (Bounds)value;
                    break;
                case SerializedPropertyType.Vector2Int:
                    serializedProperty.vector2IntValue = (Vector2Int)value;
                    break;
                case SerializedPropertyType.Vector3Int:
                    serializedProperty.vector3IntValue = (Vector3Int)value;
                    break;
                case SerializedPropertyType.RectInt:
                    serializedProperty.rectIntValue = (RectInt)value;
                    break;
                case SerializedPropertyType.BoundsInt:
                    serializedProperty.boundsIntValue = (BoundsInt)value;
                    break;
                default:
                    if (string.IsNullOrEmpty(warningMessage))
                        warningMessage += $"{serializedProperty.propertyType} serialized types aren't supported by UsePropertySetterAttribute.";
                    break;
            }
        }

        /// <summary>
        /// Tries to find the member property. Not guaranteed to find one, as the given name (or auto name) could be wrong.
        /// </summary>
        private PropertyInfo FindProperty(SerializedProperty serializedProperty)
        {
            UsePropertySetterAttribute attribute = PropertyUtility.GetAttribute<UsePropertySetterAttribute>(serializedProperty);

            string propertyName;
            if (attribute.autoFindProperty) propertyName = serializedProperty.displayName.Replace(" ", string.Empty);
            else propertyName = attribute.propertyName;
            
            Object targetObject = serializedProperty.serializedObject.targetObject;

            return ReflectionUtility.GetProperty(targetObject, propertyName);
        }

        private FieldInfo FindField(SerializedProperty serializedProperty)
        {
            Object targetObject = serializedProperty.serializedObject.targetObject;
            return ReflectionUtility.GetField(targetObject, serializedProperty.name);
        }

        //SerializedProperty.tooltip has been broken for years, so we get it manually instead.
        private string GetPropertyTooltip(SerializedProperty serializedProperty)
        {
            return PropertyUtility.GetAttribute<TooltipAttribute>(serializedProperty)?.tooltip;
        }
    }
}