using UnityEngine;

namespace NaughtyAttributes.Test
{
	[System.Serializable]
	public struct MyStruct
	{
		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 minMaxSlider;
	}

	public class NaughtyComponent : MonoBehaviour
	{
		public MyStruct myStruct;
	}
}
