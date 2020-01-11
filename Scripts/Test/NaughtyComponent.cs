using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class NaughtyComponent : MonoBehaviour
	{
		[MinMaxSlider(0, 100)]
		public Vector2 intSlider;

		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 floatSlider;
	}
}
