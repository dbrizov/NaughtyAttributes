using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class HorizontalLineTest : MonoBehaviour
    {
        [Header("Black")]
        [HorizontalLine(color: EColor.Black)]
        [Header("Blue")]
        [HorizontalLine(color: EColor.Blue)]
        [Header("Gray")]
        [HorizontalLine(color: EColor.Gray)]
        [Header("Green")]
        [HorizontalLine(color: EColor.Green)]
        [Header("Indigo")]
        [HorizontalLine(color: EColor.Indigo)]
        [Header("Orange")]
        [HorizontalLine(color: EColor.Orange)]
        [Header("Pink")]
        [HorizontalLine(color: EColor.Pink)]
        [Header("Red")]
        [HorizontalLine(color: EColor.Red)]
        [Header("Violet")]
        [HorizontalLine(color: EColor.Violet)]
        [Header("White")]
        [HorizontalLine(color: EColor.White)]
        [Header("Yellow")]
        [HorizontalLine(color: EColor.Yellow)]
        [Header("Thick")]
        [HorizontalLine(10.0f)]
        public int line0;

        public HorizontalLineNest1 nest1;
    }

    [System.Serializable]
    public class HorizontalLineNest1
    {
        [HorizontalLine]
        public int line1;

        public HorizontalLineNest2 nest2;
    }

    [System.Serializable]
    public class HorizontalLineNest2
    {
        [HorizontalLine]
        public int line2;
    }
}
