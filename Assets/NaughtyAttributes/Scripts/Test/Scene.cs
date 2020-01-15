using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class Scene : MonoBehaviour
    {
        [Scene]
        public int nextSceneIndex;

        [Scene]
        public string otherScene;
    }
}
