using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
    public class DropdownTest : MonoBehaviour
    {
        [Dropdown("intValues")]
        public int intValue;
        
        [Dropdown("intValues", DisplayPrefix = "Number: ")]
        public int prefixedIntValue;

        [Dropdown("intValues", DisplaySuffix = " Megabyte(s)")]
        public int suffixedIntValue;

        [Dropdown("intValues", "Show "," Widgets")]
        public int surroundedIntValue;

#pragma warning disable 414
        private int[] intValues = new int[] { 1, 2, 3 };
#pragma warning restore 414

        public DropdownNest1 nest1;
    }

    [System.Serializable]
    public class DropdownNest1
    {
        [Dropdown("StringValues")]
        public string stringValue;
        
        [Dropdown("StringValues", DisplayPrefix = "Letter: ")]
        public string prefixedStringValue;

        [Dropdown("StringValues", DisplaySuffix = " Grade")]
        public string suffixedStringValue;

        [Dropdown("StringValues", "Hello ", " World")]
        public string surroundedStringValue;

        private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

        public DropdownNest2 nest2;
    }

    [System.Serializable]
    public class DropdownNest2
    {
        [Dropdown("GetVectorValues")]
        public Vector3 vectorValue;
        
        [Dropdown("GetVectorValues", DisplayPrefix = "Go ")]
        public Vector3 prefixedVectorValue;

        [Dropdown("GetVectorValues", DisplaySuffix = " is the way!")]
        public Vector3 suffixedVectorValue;

        [Dropdown("GetVectorValues", "Go: ", ", now!")]
        public Vector3 surroundedVectorValue;

        private DropdownList<Vector3> GetVectorValues()
        {
            return new DropdownList<Vector3>()
            {
                { "Right", Vector3.right },
                { "Up", Vector3.up },
                { "Forward", Vector3.forward }
            };
        }
    }
}
