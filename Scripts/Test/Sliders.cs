using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class Sliders : MonoBehaviour
	{
		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider = new Vector2(0.25f, 0.75f);
	}
}
