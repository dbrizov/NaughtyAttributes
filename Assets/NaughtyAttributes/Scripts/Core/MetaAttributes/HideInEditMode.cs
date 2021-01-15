using UnityEngine;

namespace NaughtyAttributes
{
    public class HideInEditMode : ShowAttribute
    {
        public override bool Visible => Application.isPlaying;
    }
}