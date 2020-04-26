using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ProgressBarTest : MonoBehaviour
	{
		[Header("Constant ProgressBar")]
		[ProgressBar("Health", 100, EColor.Red)]
		public float health = 50.0f;

		[Header("Nested ProgressBar")]
		public ProgressBarNest1 nest1;

		[Header("Dynamic ProgressBar")]
		[ProgressBar("Elixir", "maxElixir", color: EColor.Violet)]
		public float elixir = 50.0f;
		public float maxElixir = 100.0f;
	}

	[System.Serializable]
	public class ProgressBarNest1
	{
		[ProgressBar("Mana", 100, EColor.Blue)]
		public float mana = 25.0f;

		public ProgressBarNest2 nest2;
	}

	[System.Serializable]
	public class ProgressBarNest2
	{
		[ProgressBar("Stamina", 100, EColor.Green)]
		public float stamina = 75.0f;
	}
}
