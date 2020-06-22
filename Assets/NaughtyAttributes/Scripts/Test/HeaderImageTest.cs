using UnityEngine;

namespace NaughtyAttributes.Test
{

public class HeaderImageTest : MonoBehaviour
{
  public const string ICON
    = "Assets/NaughtyAttributes/Samples/DemoScene/TestAssets/icon-github.png";
  [HeaderImage(ICON)]
  public Sprite sprite0;

  [HeaderImage(ICON, 96, 96)]
  public GameObject prefab0;

  [HeaderImage(ICON, 96, 96, EAlignment.Center)]
  public GameObject prefab1;

  [HeaderImage(ICON, 96, 96, EAlignment.Right)]
  public GameObject prefab2;

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
