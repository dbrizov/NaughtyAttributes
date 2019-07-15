using UnityEngine;
using NaughtyAttributes;

public class GetSetProperties : MonoBehaviour
{
    [SerializeField, GetSet("MyInt")]
    private int _MyInt;
    public int MyInt
    {
        get
        {
            print("Getting");
            return _MyInt;
        }
        set
        {
            print("Setting");
            _MyInt = value;
        }
    }


    [Button]
    private void IncrementProperty()
    {
        MyInt++;
    }

    [Button]
    private void ReadProperty()
    {
        var i = MyInt;
    }
}
