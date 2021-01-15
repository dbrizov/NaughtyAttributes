using UnityEngine;

namespace NaughtyAttributes
{
    public class ShowInEditMode : ShowAttribute
    {
        public override bool Visible => !Application.isPlaying;
    }
}