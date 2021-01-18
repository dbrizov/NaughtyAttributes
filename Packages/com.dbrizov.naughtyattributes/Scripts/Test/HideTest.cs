using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HideTest : MonoBehaviour
    {
        [HideInEditMode]
        public bool hideInEditMode;

        public bool alwaysVisible;
        
        [HideInPlayMode]
        public bool hideInPlayMode;

    }
}