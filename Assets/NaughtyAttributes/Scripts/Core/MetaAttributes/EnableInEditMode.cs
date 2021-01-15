using UnityEngine;

namespace NaughtyAttributes
{
    public class EnableInEditMode : EnabledAttribute
    {
        public override bool Enabled => !Application.isPlaying;
    }
}