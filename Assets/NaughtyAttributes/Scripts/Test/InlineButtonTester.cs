using NaughtyAttributes;
using UnityEngine;

public class InlineButtonTester : MonoBehaviour
{
    [SerializeField]
    [InlineButton("MyInt")]
    private int _MyInt;

    [SerializeField]
    [InlineButton("MyFloat", "A_Float")]
    private float _MyFloat;

    [SerializeField]
    [InlineButton("MyString", expandButton: true)]
    private string _MyString;

    [SerializeField]
    [InlineButton("MyVector3", "A_Vec3", true)]
    private Vector3 _MyVector3;

    private void MyInt()
    {
        Debug.Log(_MyInt);
    }

    private void MyFloat()
    {
        Debug.Log(_MyFloat);
    }

    private void MyString()
    {
        Debug.Log(_MyString);
    }

    private void MyVector3()
    {
        Debug.Log(_MyVector3);
    }

}
