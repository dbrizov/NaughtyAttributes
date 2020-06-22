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
  public int? Width { get; private set; }
  public int? Height { get; private set; }
  public EAlignment Alignment { get; private set; }

  public HeaderImageAttribute(string assetPath, EAlignment alignment = EAlignment.Left)
  {
    Path = assetPath;
    Alignment = alignment;
  }

  public HeaderImageAttribute(string assetPath, int width, EAlignment alignment = EAlignment.Left) : this(assetPath, width, width, alignment) { }

  public HeaderImageAttribute(string assetPath, int width, int height, EAlignment alignment = EAlignment.Left)
  {
    Path = assetPath;
    Width = width;
    Height = height;
    Alignment = alignment;
  }
}
}
