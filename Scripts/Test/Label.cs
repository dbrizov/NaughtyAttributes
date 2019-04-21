using UnityEngine;
using NaughtyAttributes;
using UnityEngine.AI;

public class Label : MonoBehaviour
{
    [Label("A Short Name")]
    public string aMoreSpecificName;

    [Label("RGB")]
    public Vector3 vectorXYZ;

    [Label("Agent")]
    public NavMeshAgent navMeshAgent;

    [Label("Ints")]
    public int[] arrayOfInts;

    [Label("Custom Class")]
    public MyClassExample myClass;

    [System.Serializable]
    public class MyClassExample
    {
        public int aInt;
        public string aString;
    }
}
