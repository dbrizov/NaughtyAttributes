using UnityEngine;
using System.Collections;
using NaughtyAttributes;

[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/NaughtyScriptableObject")]
public class NaughtyScriptableObject : ScriptableObject
{
    [Header("integer")]
    public int integer;

    [Header("sprite")]
    [ShowAssetPreview]
    public Sprite sprite;

    [Header("zero")]
    [ReadOnly]
    public Vector3 zero = Vector3.zero;

    private int[] values = new int[] { 1, 2, 3, 4 };

    [Header("dropdown")]
    [Dropdown("values")]
    public int dropdown;

    [Header("minMaxSlider")]
    [MinMaxSlider(0, 1)]
    public Vector2 minMaxSlider;

    [Header("slider")]
    [Slider(0, 10)]
    public int slider;

    [Header("health")]
    [ProgressBar]
    public float health = 50;

    [Header("list")]
    [ReorderableList]
    public float[] list;

    [Header("textArea")]
    [ResizableTextArea]
    public string textArea;
}
