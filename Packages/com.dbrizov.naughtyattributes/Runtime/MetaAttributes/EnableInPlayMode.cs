using UnityEngine;

namespace NaughtyAttributes
{
    public class EnableInPlayMode : EnabledAttribute
    {
        public override bool Enabled => Application.isPlaying;
    }
}