using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class SliderTest : MonoBehaviour
    {
        [Slider(0.0f, 1.0f)]
        public float slider0 = 0.25f;
        [Slider(0, 10)]
        public int intSlider0 = 1;

        public SliderNest1 nest1;
    }

    [System.Serializable]
    public class SliderNest1
    {
        [Slider(0.0f, 1.0f)]
        public float slider1 = 0.25f;
        [Slider(0, 10)]
        public int intSlider1 = 1;

        public SliderNest2 nest2;
    }

    [System.Serializable]
    public class SliderNest2
    {
        [Slider(0.0f, 1.0f)]
        public float slider2 = 0.25f;
        [Slider(0, 10)]
        public int intSlider2 = 1;
    }
}
