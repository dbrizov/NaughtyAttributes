using NaughtyAttributes;
using UnityEngine;

public class NaughtyComponent : MonoBehaviour
{
    [BoxGroup("A")]
    [Dropdown("intValues")]
    public int intValue;

    [BoxGroup("B")]
    [Dropdown("stringValues")]
    public string stringValue;

    [BoxGroup("A")]
    [Dropdown("vectorValues")]
    public Vector3 vectorValue;

    [BoxGroup("C")]
    [ReorderableList]
    public int[] intArray;

#pragma warning disable 414
    private int[] intValues = new int[] { 1, 2, 3, 4, 5 };

    private string[] stringValues = new string[] { "A", "B", "C", "D", "E" };

    private DropdownList<Vector3> vectorValues = new DropdownList<Vector3>()
    {
        { "Right",   Vector3.right   },
        { "Left",    Vector3.left    },
        { "Up",      Vector3.up      },
        { "Down",    Vector3.down    },
        { "Forward", Vector3.forward },
        { "Back",    Vector3.back    },
    };
#pragma warning restore 414

    [Button]
    private void Method()
    {
        Debug.Log("Method");
    }
}
