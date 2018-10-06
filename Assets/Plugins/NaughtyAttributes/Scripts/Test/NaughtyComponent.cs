using NaughtyAttributes;
using System.Collections.Generic;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [BoxGroup("Sliders")]
    [Slider(0, 10)]
    public int intSlider;

    [BoxGroup("Sliders")]
    [Slider(0.0f, 10.0f)]
    public float floatSlider;

    [BoxGroup("Sliders")]
    [MinMaxSlider(0.0f, 100.0f)]
    public Vector2 minMaxSlider;

    [BoxGroup("Reorderable Lists")]
    [ReorderableList]
    public int[] intArray;

    [BoxGroup("Reorderable Lists")]
    [ReorderableList]
    public List<Vector3> vectorList;

    [BoxGroup("Dropdowns")]
    [Dropdown("intValues")]
    public int intValue;

    [BoxGroup("Dropdowns")]
    [Dropdown("stringValues")]
    public string stringValue;

    [BoxGroup("Dropdowns")]
    [Dropdown("vectorValues")]
    public Vector3 vectorValue;

#pragma warning disable 414
    private int[] intValues = new int[] { 1, 2, 3 };

    private List<string> stringValues = new List<string>() { "A", "B", "C" };

    private DropdownList<Vector3> vectorValues = new DropdownList<Vector3>()
    {
        { "Right", Vector3.right },
        { "Up", Vector3.up },
        { "Forward", Vector3.forward }
    };
#pragma warning restore 414

    [ResizableTextArea]
    public string textArea;

#pragma warning disable 414
    [ShowNonSerializedField]
    private int myInt = 10;

    [ShowNonSerializedField]
    private const float PI = 3.14159f;

    [ShowNonSerializedField]
    private static readonly Vector3 CONST_VECTOR = Vector3.one;
#pragma warning restore 414

    [ShowNativeProperty]
    public Transform Transform
    {
        get
        {
            return this.transform;
        }
    }

    [ReadOnly]
    public int readOnlyInt = 5;

    public bool enableInt = true;

    [EnableIf("enableInt")]
    public int enableIf;

    [DisableIf("enableInt")]
    public int disabledIf;

    [EnableIf("ReturnTrue")]
    public int enabledInt;

    [DisableIf("ReturnTrue")]
    public int disabledInt;

    public bool showInt = true;

    [ShowIf("showInt")]
    public int showIf;

    [HideIf("showInt")]
    public int hideIf;

    [ShowIf("ReturnTrue")]
    public int shownInt;

    [HideIf("ReturnTrue")]
    public int hiddenInt;

    [ShowAssetPreview]
    public Sprite sprite;

    [ShowAssetPreview(96, 96)]
    public GameObject prefab;

    [ProgressBar("Health", 100, ProgressBarColor.Orange)]
    public float health = 50;

    [MinValue(0.0f), MaxValue(1.0f)]
    public float minMaxValidated;

    [Required]
    public Transform requiredTransform;

    [Required("Must not be null")]
    public GameObject requiredGameObject;

    [ValidateInput("IsNotNull", "must not be null")]
    public Sprite notNullSprite;

    [InfoBox("Has onValueChanged callback")]
    [OnValueChanged("OnValueChanged")]
    public int onValueChanged;

    [Button]
    public void MethodOne()
    {
        Debug.Log("MethodOne()");
    }

    [Button("Button Text")]
    private void MethodTwo()
    {
        Debug.Log("MethodTwo()");
    }

    private bool ReturnTrue()
    {
        return true;
    }

    private bool IsNotNull(Sprite sprite)
    {
        return sprite != null;
    }

    private void OnValueChanged()
    {
        Debug.Log(this.onValueChanged);
    }
}
