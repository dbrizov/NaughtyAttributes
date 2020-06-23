using System;

namespace NaughtyAttributes
{
public enum EAlignment {
  Left,
  Center,
  Right
}

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class HeaderImageAttribute : DrawerAttribute
{

  public string Path { get; private set; }

  /** What I'd really like here is

      ```
      public int? Width { get; private set; }
      ```

      however, int? is not acceptable for attribute constructors. So we do this
      circumlocution with Width being expected by the attribute constructor
      namely:

      ```
      [HeaderImage(ICON_PATH, Width = 10)]
      public int parameter;
      ```

      And then in our calling code we access the `width` property. Would prefer
      `width` to be marked internal instead of public, but its calling code is in
      another namespace.
  */
  private int _width = -1;
  public int Width { get => _width; set => _width = value; }
  public int? width => _width >= 0 ? _width : (int?) null;

  private int _height = -1;
  public int Height { get => _height; set => _height = value; }
  public int? height => _height >= 0 ? _height : (int?) null;

  public EAlignment Alignment { get; set; }

  public HeaderImageAttribute(string assetPath, EAlignment alignment = EAlignment.Left)
  {
    Path = assetPath;
    Alignment = alignment;
  }

  public HeaderImageAttribute(string assetPath, int width, int height, EAlignment alignment = EAlignment.Left)
  {
    Path = assetPath;
    if (width >= 0)
    {
      Width = width;
    }
    if (height >= 0)
    {
      Height = height;
    }
    Alignment = alignment;
  }
}
}
