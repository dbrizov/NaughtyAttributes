using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    //[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
    public class _NaughtyScriptableObject : ScriptableObject
    {
        [Expandable]
        public List<_TestScriptableObjectA> listA;

        public int myInt;

        [Button(enabledMode: EButtonEnableMode.Always)]
        private void IncrementMyInt()
        {
            myInt++;
        }

        [Button("Decrement My Int", EButtonEnableMode.Editor)]
        private void DecrementMyInt()
        {
            myInt--;
        }
    }
}
