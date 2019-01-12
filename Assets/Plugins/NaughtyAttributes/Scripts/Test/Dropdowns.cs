using UnityEngine;
using NaughtyAttributes;
using System.Collections.Generic;

public class Dropdowns : MonoBehaviour
{
    [Dropdown("intValues")]
    public int intValue;

    [Dropdown("stringValues")]
    public string stringValue;

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
}
