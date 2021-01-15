using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class EnableTest : MonoBehaviour
    {
        [DisabledInPlayMode]
        public bool disableInEditMode;
		
        [DisableInEditMode]
        public bool disableInPlayMode;
    }
}