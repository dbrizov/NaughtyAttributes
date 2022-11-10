using UnityEngine;

namespace NaughtyAttributes.Test
{
    public interface IRequiredTypeTestInterface
    {
    }

    public interface IRequiredTypeTestInterface2
    {
    }

    public class RequiredTypeTest : MonoBehaviour
    {
        [RequiredType(typeof(Rigidbody))] public GameObject gameObjectMustHaveRigidbody;
        [RequiredType(typeof(Rigidbody))] public Transform transformMustHaveRigidbody;

        [RequiredType(typeof(IRequiredTypeTestInterface))]
        public GameObject gameObjectMustHaveInterface;

        [RequiredType(typeof(RequiredTypeTestTestObject))]
        public GameObject gameObjectMustHaveComponent;

        [RequiredType(showInfoMessageWhenEmpty: false, typeof(IRequiredTypeTestInterface))]
        public GameObject shouldNotShowInfoMessageWhenEmpty;

        [RequiredType(typeof(IRequiredTypeTestInterface), typeof(IRequiredTypeTestInterface2))] public GameObject gameObjectMustHaveMultipleType;
    }
}