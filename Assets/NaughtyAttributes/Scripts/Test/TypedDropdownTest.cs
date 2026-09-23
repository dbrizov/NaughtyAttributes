#if UNITY_2021_3_OR_NEWER
using NaughtyAttributes;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class TypedDropdownTest : MonoBehaviour
    {
        [SerializeReference]
        [TypeDropdown(typeof(Animal))]
        public Animal MyPet;
    }

    [System.Serializable]
    public class Animal
    {
        public string nickname = "Silly Billy";
    }

    public class Dog : Animal
    {
        public string Breed = "Golden Retriever";
    }

    public class Cat : Animal
    {
        public int lives = 9;
    }
}
#endif
