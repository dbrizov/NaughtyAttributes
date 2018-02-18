# NaughtyAttributes
Attribute Extensions for Unity

## System Requirements
Unity 2017.1.0 or later versions. Feel free to try older version

## Drawer Attributes
Provide special draw options to serialized fields.
A field can have only one DrawerAttribute. If a field has more than one, only the bottom one will be used.

### Slider
The same as Unity's **Range** attribute.
There is no difference between the two, you can use whatever you like, I just wanted to support a custom slider attribute.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Slider_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Slider_Inspector.PNG)

### MinMaxSlider
A double slider. The **min value** is saved to the **X** property, and the **max value** is saved to the **Y** property of a **Vector2** field.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/MinMaxSlider_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/MinMaxSlider_Inspector.PNG)

### ReorderableList
Provides array type fields with an interface for easy reordering of elements.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ReorderableList_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ReorderableList_Inspector.gif)

### Button
A method can be marked as a button. A button appears in the inspector and executes the method if clicked.
Works both with instance and static methods.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Button_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Button_Inspector.PNG)

### Dropdown
Provides an interface for dropdown value selection.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Dropdown_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Dropdown_Inspector.gif)

### ResizableTextArea
A resizable text area where you can see the whole text.
Unlike Unity's **Multiline** and **TextArea** attributes where you can see only 3 rows of a given text, and in order to see it or modify it you have to manually scroll down to the desired row.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ResizableTextArea_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ResizableTextArea_Inspector.gif)

### ShowNonSerializedField
Shows non-serialized fields in the inspector.
All non-serialized fields are displayed at the botton of the inspector before the method buttons.
Keep in mind that if you change a non-static non-serialized field in the code - the value in the inspector will be updated after you press **Play** in the editor.
There is no such issue with static non-serialized fields because their values are updated at compile time.
It also supports only value types and string **(int, long, float, double, string, Vector2, Vector3, Vector4, Color, Bounds, Rect)**. 

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowNonSerializedField_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowNonSerializedField_Inspector.PNG)

### ShowNativeProperty
Shows native C# properties in the inspector.
All native properties are displayed at the bottom of the inspector after the non-serialized fields and before the method buttons.
It also supports only value types and string **(int, long, float, double, string, Vector2, Vector3, Vector4, Color, Bounds, Rect)**.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowNativeProperty_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowNativeProperty_Inspector.PNG)

### ReadOnly
Makes a field read only.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ReadOnly_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ReadOnly_Inspector.PNG)

### ShowAssetPreview
Shows the texture preview of a given asset (Sprite, Prefab...)

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowAssetPreview_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowAssetPreview_Inspector.PNG)

### ProgressBar
Thanks to [Shinao](https://github.com/Shinao)

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ProgressBar_Code.png)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ProgressBar_Inspector.gif)

## DrawCondition Attributes
Can be used to specify when a given serialized field is visible, and when not. A field can have only one DrawConditionAttribute.

### ShowIf / HideIf
![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowIf_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ShowIf_Inspector.gif)

## Group Attributes
Serialized fields can be grouped in different groups.
A field can have only one GroupAttribute. If a field has more than one, only the bottom one will be used.

### BoxGroup
Surrounds grouped fields with a box.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/BoxGroup_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/BoxGroup_Inspector.PNG)

## Validator Attributes
Used for validating the fields. A field can have infinite number of validator attributes.

### MinValue / MaxValue
Clamps integer and float fields.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/MinValueMaxValue_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/MinValueMaxValue_Inspector.gif)

### Required
Used to remind the developer that a given reference type field is required.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Required_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/Required_Inspector.PNG)

### ValidateInput
The most powerful ValidatorAttribute.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ValidateInput_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/ValidateInput_Inspector.PNG)

## Meta Attributes
Give the fields meta data. A field can have infinite number of meta attributes.

### InfoBox
Used for providing additional information.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/InfoBox_Code.PNG)

![inspector](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/InfoBox_Inspector.PNG)

### OnValueChanged
Detects a value change and executes a callback.
Keep in mind that the event is detected only when the value is changed from the inspector.
If you want a runtime event, you should probably use an event/delegate and subscribe to it.

![code](https://github.com/dbrizov/NaughtyAttributes/blob/master/Assets/Plugins/NaughtyAttributes/Documentation/OnValueChanged_Code.PNG)

## How to create your own attributes
Lets say you want to implement your own **[ReadOnly]** attribute.

First you have to create a **ReadOnlyAttribute** class
```
[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ReadOnlyAttribute : DrawerAttribute
{
}
```

Then you need to create a drawer for that attribute
```
[PropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyPropertyDrawer : PropertyDrawer
{
	public override void DrawProperty(SerializedProperty property)
	{
		GUI.enabled = false;
		EditorGUILayout.PropertyField(property, true);
		GUI.enabled = true;
	}
}
```

Last, in order for the editor to recognize the drawer for this attribute, you have to press the **NaughtyAttributes/Update Attributes Database** menu item in the editor.

## License
MIT License

Copyright (c) 2017 Denis Rizov

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
