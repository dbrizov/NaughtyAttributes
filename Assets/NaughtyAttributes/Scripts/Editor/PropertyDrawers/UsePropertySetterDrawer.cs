using System.Reflection;
using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(UsePropertySetterAttribute))]
    public class UsePropertySetterDrawer : PropertyDrawer
    {
        //Cached objects
        readonly private GUIContent noLabel = GUIContent.none;
        private GUIContent labelContent = new GUIContent();


        public override void DrawProperty(SerializedProperty serializedProperty)
        {
            string warningMessage = "";

            //Draw the header if it has one
            EditorDrawUtility.DrawHeader(serializedProperty);

            //Find the property member
            PropertyInfo property = FindProperty(serializedProperty);
            if (property == null)
            {
                EditorDrawUtility.DrawHelpBox($"No setter was found for member {serializedProperty.name}.",
                    MessageType.Error, logToConsole: false, context: serializedProperty.serializedObject.context);
                return;
            }

            //Draw the appropiate field and get the value entered by the user in the inspector
            object inspectorValue;            
            bool valueChanged = DrawControl(serializedProperty, out inspectorValue, ref warningMessage);

            //If the value changed, call the setter, read the resulting value and set the serialized property to that value
            if (valueChanged)
            {
                Undo.RecordObject(serializedProperty.serializedObject.targetObject, $"Changed {serializedProperty.displayName} in inspector.");
                property.SetValue(serializedProperty.serializedObject.targetObject, inspectorValue);

                object processedValue = FindField(serializedProperty).GetValue(serializedProperty.serializedObject.targetObject);
                ModifySerializedValue(serializedProperty, processedValue);
                serializedProperty.serializedObject.ApplyModifiedProperties();
            }

            //If we have a warning message, show it.
            if (warningMessage != "")
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
            //Gather the needed stuff before drawing the property
            bool isOverridenProperty = serializedProperty.isInstantiatedPrefab && serializedProperty.prefabOverride;
            System.Type memberType = ReflectionUtility.GetField(serializedProperty.serializedObject.targetObject, serializedProperty.name).FieldType;

            //Prepare the label
            labelContent.text = serializedProperty.displayName;
            labelContent.tooltip = GetPropertyTooltip(serializedProperty);

            //Attributes we may neeed to look for
            RangeAttribute rangeAttribute = null;
            DelayedAttribute delayedAttribute = null;
            
            object oldValue = null;
            newValue = null;
            bool valueChanged = false;

            //Draw the label, make it bold if it's a prefab override.
            //The exact following style given doesn't seem to matter much as long as it's a field.
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel(labelContent, EditorStyles.objectField, isOverridenProperty ? EditorStyles.boldLabel : EditorStyles.label);

            //Draw the property correctly depending on the serialized type.
            //Get its value.
            //Decide wether the value changed.
            switch (serializedProperty.propertyType)
            {
                case SerializedPropertyType.Integer:
                    rangeAttribute = PropertyUtility.GetAttribute<RangeAttribute>(serializedProperty);
                    delayedAttribute = PropertyUtility.GetAttribute<DelayedAttribute>(serializedProperty);

                    int inputIntValue;
                    if (rangeAttribute != null) inputIntValue = EditorGUILayout.IntSlider(serializedProperty.intValue, (int)rangeAttribute.min, (int)rangeAttribute.max);
                    else if (delayedAttribute != null) inputIntValue = EditorGUILayout.DelayedIntField(noLabel, serializedProperty.intValue);
                    else inputIntValue = EditorGUILayout.IntField(noLabel, serializedProperty.intValue);

                    newValue = inputIntValue;
                    valueChanged = inputIntValue != serializedProperty.intValue;
                    break;

                case SerializedPropertyType.Boolean:
                    bool inputBoolValue = EditorGUILayout.Toggle(noLabel, serializedProperty.boolValue);
                    newValue = inputBoolValue;
                    valueChanged = inputBoolValue != serializedProperty.boolValue;
                    break;

                case SerializedPropertyType.Float:
                    rangeAttribute = PropertyUtility.GetAttribute<RangeAttribute>(serializedProperty);
                    delayedAttribute = PropertyUtility.GetAttribute<DelayedAttribute>(serializedProperty);

                    float newFloatValue;
                    if (rangeAttribute != null) newFloatValue = EditorGUILayout.Slider(noLabel, serializedProperty.floatValue, rangeAttribute.min, rangeAttribute.max);
                    else if (delayedAttribute != null) newFloatValue = EditorGUILayout.DelayedFloatField(noLabel, serializedProperty.floatValue);
                    else newFloatValue = EditorGUILayout.FloatField(noLabel, serializedProperty.floatValue);

                    newValue = newFloatValue;
                    valueChanged = newFloatValue != serializedProperty.floatValue;
                    break;

                case SerializedPropertyType.String:
                    delayedAttribute = PropertyUtility.GetAttribute<DelayedAttribute>(serializedProperty);
                    MultilineAttribute multilineAttribute = PropertyUtility.GetAttribute<MultilineAttribute>(serializedProperty);
                    TextAreaAttribute textAreaAttribute = PropertyUtility.GetAttribute<TextAreaAttribute>(serializedProperty);

                    if (multilineAttribute != null) warningMessage += "MultilineAttribute is not supported by UsePropertySetterAttribute.\n\n";
                    if (textAreaAttribute != null) warningMessage += "TextAreaAttribute is not supported by UsePropertySetterAttribute.\n\n";

                    string newStringValue;
                    if (delayedAttribute != null) newStringValue = EditorGUILayout.DelayedTextField(noLabel, serializedProperty.stringValue);
                    else newStringValue = EditorGUILayout.TextField(noLabel, serializedProperty.stringValue);

                    newValue = newStringValue;
                    valueChanged = newStringValue != serializedProperty.stringValue;
                    break;

                case SerializedPropertyType.Color:
                    ColorUsageAttribute colorUsageAttribute = PropertyUtility.GetAttribute<ColorUsageAttribute>(serializedProperty);

                    Color newColorValue;
                    if (colorUsageAttribute != null)
                    {
                        newColorValue = EditorGUILayout.ColorField(noLabel, serializedProperty.colorValue, true, 
                            colorUsageAttribute.showAlpha, colorUsageAttribute.hdr);
                    }
                    else
                    {
                        newColorValue = EditorGUILayout.ColorField(noLabel, serializedProperty.colorValue);
                    }

                    newValue = newColorValue;
                    valueChanged = newColorValue != serializedProperty.colorValue;
                    break;

                case SerializedPropertyType.ObjectReference:
                    oldValue = serializedProperty.objectReferenceValue;
                    newValue = EditorGUILayout.ObjectField(noLabel, serializedProperty.objectReferenceValue, memberType,
                        !EditorUtility.IsPersistent(serializedProperty.serializedObject.targetObject));

                    valueChanged = oldValue != newValue;
                    break;

                case SerializedPropertyType.Enum:
                    System.Array enumValues = memberType.GetEnumValues();
                    if (serializedProperty.enumValueIndex == -1)
                    {
                        warningMessage += "This enum is not supported by UsePropertySetterAttribute. Enum support is a bit iffy. Sorry.\n\n";
                    }
                    else
                    {
                        System.Enum selected = (System.Enum)enumValues.GetValue(serializedProperty.enumValueIndex);

                        System.Enum newEnumValue = EditorGUILayout.EnumPopup(noLabel, selected);

                        newValue = newEnumValue;
                        //Comparing the System.Enum values directly gave false positives every frame, so we compare strings.
                        valueChanged = newEnumValue.ToString() != selected.ToString();  
                    }
                    break;

                case SerializedPropertyType.Vector2:
                    Vector2 newVector2Value = EditorGUILayout.Vector2Field(noLabel, serializedProperty.vector2Value);

                    newValue = newVector2Value;
                    valueChanged = newVector2Value != serializedProperty.vector2Value;
                    break;

                case SerializedPropertyType.Vector3:
                    Vector3 newVector3Value = EditorGUILayout.Vector3Field(noLabel, serializedProperty.vector3Value);

                    newValue = newVector3Value;
                    valueChanged = newVector3Value != serializedProperty.vector3Value;
                    break;
                case SerializedPropertyType.Vector4:
                    Vector4 newVector4Value = EditorGUILayout.Vector4Field(noLabel, serializedProperty.vector4Value);

                    newValue = newVector4Value;
                    valueChanged = newVector4Value != serializedProperty.vector4Value;
                    break;
                case SerializedPropertyType.Rect:
                    Rect newRectValue = EditorGUILayout.RectField(noLabel, serializedProperty.rectValue);

                    newValue = newRectValue;
                    valueChanged = newRectValue != serializedProperty.rectValue;
                    break;

                case SerializedPropertyType.AnimationCurve:
                    AnimationCurve newAnimationCurveValue = EditorGUILayout.CurveField(noLabel, serializedProperty.animationCurveValue);

                    newValue = newAnimationCurveValue;
                    valueChanged = newAnimationCurveValue != serializedProperty.animationCurveValue;
                    break;

                case SerializedPropertyType.Bounds:
                    Bounds newBoundsValue = EditorGUILayout.BoundsField(noLabel, serializedProperty.boundsValue);

                    newValue = newBoundsValue;
                    valueChanged = newBoundsValue != serializedProperty.boundsValue;
                    break;

                case SerializedPropertyType.Vector2Int:
                    Vector2Int newVector2IntValue = EditorGUILayout.Vector2IntField(noLabel, serializedProperty.vector2IntValue);

                    newValue = newVector2IntValue;
                    valueChanged = newVector2IntValue != serializedProperty.vector2IntValue;
                    break;

                case SerializedPropertyType.Vector3Int:
                    Vector3Int newVector3IntValue = EditorGUILayout.Vector3IntField(noLabel, serializedProperty.vector3IntValue);

                    newValue = newVector3IntValue;
                    valueChanged = newVector3IntValue != serializedProperty.vector3IntValue;
                    break;
                case SerializedPropertyType.RectInt:
                    RectInt newRectIntValue = EditorGUILayout.RectIntField(noLabel, serializedProperty.rectIntValue);

                    newValue = newRectIntValue;
                    valueChanged = newRectIntValue.Equals(serializedProperty.rectIntValue);
                    break;

                case SerializedPropertyType.BoundsInt:
                    BoundsInt newBoundsIntValue = EditorGUILayout.BoundsIntField(noLabel, serializedProperty.boundsIntValue);

                    newValue = newBoundsIntValue;
                    valueChanged = newBoundsIntValue != serializedProperty.boundsIntValue;
                    break;

                default:
                    warningMessage += $"{serializedProperty.propertyType} serialized types aren't supported by UsePropertySetterAttribute. \n\n";
                    break;
            }
            EditorGUILayout.EndHorizontal();

            return valueChanged;
        }

        /// <summary>
        /// Sets the value at the serialized version.
        /// </summary>
        private void ModifySerializedValue(SerializedProperty serializedProperty, object value)
        {
            //Assign the value to the serialized version in the correct way depending on each serialized type
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
                    break;
            }

            //Set the object dirty so that the editor picks up the changes on the prefab
            EditorUtility.SetDirty(serializedProperty.serializedObject.targetObject);
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