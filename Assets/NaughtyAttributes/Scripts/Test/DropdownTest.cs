using UnityEngine;
using System.Collections.Generic;

namespace NaughtyAttributes.Test
{
    public class DropdownTest : MonoBehaviour
    {
        [Dropdown("intValues")]
        public int intValue;
        
        [Dropdown("intValues", "Number: {0}")]
        public int prefixedIntValue;

        [Dropdown("intValues", "{0} Megabyte(s)")]
        public int suffixedIntValue;

        [Dropdown("intValues", "Show {0} Widgets")]
        public int surroundedIntValue;
        
        [Dropdown("floatValues", "Value: {0}")]
        public float floatValue;
        
        [Dropdown("floatValues", "{0:0.#%}")]
        public float formattedFloatValue;

#pragma warning disable 414
        private int[] intValues = new int[] { 1, 2, 3 };
        private float[] floatValues = new float[] { 0.1234f, 0.5648f, 1.0f };
#pragma warning restore 414

        public DropdownNest1 nest1;
    }

    [System.Serializable]
    public class DropdownNest1
    {
        [Dropdown("StringValues")]
        public string stringValue;
        
        [Dropdown("StringValues", "Letter: {0}")]
        public string prefixedStringValue;

        [Dropdown("StringValues", "{0} Grade")]
        public string suffixedStringValue;

        [Dropdown("StringValues", "Hello {0} World")]
        public string surroundedStringValue;

        private List<string> StringValues { get { return new List<string>() { "A", "B", "C" }; } }

        public DropdownNest2 nest2;
    }

    [System.Serializable]
    public class DropdownNest2
    {
        [Dropdown("GetVectorValues")]
        public Vector3 vectorValue;
        
        [Dropdown("GetVectorValues", "Go {0}")]
        public Vector3 prefixedVectorValue;

        [Dropdown("GetVectorValues", "{0} is the way!")]
        public Vector3 suffixedVectorValue;

        [Dropdown("GetVectorValues", "Go: {0} now!")]
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
