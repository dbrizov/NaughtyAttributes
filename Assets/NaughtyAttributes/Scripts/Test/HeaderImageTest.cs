using UnityEngine;

namespace NaughtyAttributes.Test
{

public class HeaderImageTest : MonoBehaviour
{
  public const string ICON0
    = "Assets/NaughtyAttributes/Samples/DemoScene/TestAssets/icon-github.png";
  public const string ICON
    = "Assets/NaughtyAttributes/Samples/DemoScene/TestAssets/quadratic.png";
  [HeaderImage(ICON, EAlignment.Center)]
  public Sprite sprite0;

  [HeaderImage(ICON, 96, 96)]
  public float a;

  [HeaderImage(ICON, Height = 96)]
  public float a1;

  [HeaderImage(ICON0, Width = 96)]
  public float a0;

  [HeaderImage(ICON, 96, 96, EAlignment.Center)]
  public float b;

  [HeaderImage(ICON, Width = 96, Alignment = EAlignment.Center)]
  public float b1;

  [HeaderImage(ICON, Height = 96, Alignment = EAlignment.Center)]
  public float b2;

  [HeaderImage(ICON0, 96, 96, EAlignment.Center)]
  public float b0;

  [HeaderImage(ICON, 96, 96, EAlignment.Right)]
  public float c;

  [HeaderImage(ICON, Width = 96, Alignment = EAlignment.Right)]
  public float c1;

  [HeaderImage(ICON, Height = 96, Alignment = EAlignment.Right)]
  public float c2;

  [HeaderImage(ICON0, 96, 96, EAlignment.Right)]
  public float c0;

  public HeaderImageNest1 nest1;
}

[System.Serializable]
public class HeaderImageNest1
{
  [HeaderImage(HeaderImageTest.ICON)]
  public Sprite sprite1;

  [HeaderImage(HeaderImageTest.ICON, 96, 96)]
  public GameObject prefab0;

  [HeaderImage(HeaderImageTest.ICON, 96, 96, EAlignment.Center)]
  public GameObject prefab1;

  [HeaderImage(HeaderImageTest.ICON, 96, 96, EAlignment.Right)]
  public GameObject prefab2;
  public HeaderImageNest2 nest2;
}

[System.Serializable]
public class HeaderImageNest2
{
  [HeaderImage(HeaderImageTest.ICON)]
  public Sprite sprite2;

  [HeaderImage(HeaderImageTest.ICON, 96, 96)]
  public GameObject prefab0;

  [HeaderImage(HeaderImageTest.ICON, 96, 96, EAlignment.Center)]
  public GameObject prefab1;

  [HeaderImage(HeaderImageTest.ICON, 96, 96, EAlignment.Right)]
  public GameObject prefab2;
}
}
