using UnityEngine;

namespace NaughtyAttributes
{
    public class DisableInEditMode : EnabledAttribute
    {
        public override bool Enabled => Application.isPlaying;
    }
}