using UnityEngine;

namespace NaughtyAttributes
{
    public class ShowInPlayMode : ShowAttribute
    {
        public override bool Visible => Application.isPlaying;
    }
}