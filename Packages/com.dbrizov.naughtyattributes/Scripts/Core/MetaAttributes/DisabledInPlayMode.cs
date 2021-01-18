using UnityEngine;

namespace NaughtyAttributes
{
    public class DisabledInPlayMode : EnabledAttribute
    {
        public override bool Enabled => !Application.isPlaying;
    }
}