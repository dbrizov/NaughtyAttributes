Orginal Repo: https://github.com/dbrizov/NaughtyAttributes

Here I added more features. I already requested pulls, but to be sure I will describe what is new here.

# NaughtyAttributes
NaughtyAttributes is an extension for the Unity Inspector.

It expands the range of attributes that Unity provides so that you can create powerful inspectors without the need of custom editors or property drawers. It also provides attributes that can be applied to non-serialized fields or functions.

It is implemented by replacing the default Unity Inspector. This means that if you have any custom editors, NaughtyAttributes will not work with them. All of your custom editors and property drawers are not affected in any way.

## System Requirements
Unity 2017.1.0 or later versions. Feel free to try older version. Don't forget to include the NaughtyAttributes namespace.

## Drawer Attributes
Provide special draw options to serialized fields.
A field can have only one DrawerAttribute. If a field has more than one, only the bottom one will be used.

### Button - Enabled / Disabled
Original **Button** looks like so

![code](https://github.com/MaZyGer/NaughtyAttributes/blob/master/Assets/NaughtyAttributes/Documentation/Button_Code.PNG)

![inspector](https://github.com/MaZyGer/NaughtyAttributes/blob/master/Assets/NaughtyAttributes/Documentation/Button_Inspector.PNG)

New features: You can add **activeInEditMode** or **activeInPlayMode**. By default they are *true*. You can use them to make Buttons enabled in editmode or/and playmode. This will help to avoid some errors if is only possible in editor or playmode.
If **activeInEditMode** = false so prefabs will also disabled Button.

![code](https://i.imgur.com/CHYI860.png)

![inspector](https://i.imgur.com/Ww2kPvA.png)

### ProgressBar - with property / field reader 

Original. You had to set the number by yourself. In this case 25.

![code](https://github.com/MaZyGer/NaughtyAttributes/blob/master/Assets/NaughtyAttributes/Documentation/ProgressBar_Code.png)

The new added feature will read also fields or properties.
![code](https://i.imgur.com/z0clxzP.png)


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
