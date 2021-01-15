using UnityEngine;

namespace NaughtyAttributes
{
    public class HideInPlayMode : ShowAttribute
    {
        public override bool Visible => !Application.isPlaying;
    }
}