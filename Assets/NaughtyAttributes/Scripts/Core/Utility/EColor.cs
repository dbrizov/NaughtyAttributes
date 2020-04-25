using UnityEngine;

namespace NaughtyAttributes
{
	public enum EColor
	{
		Clear,
		White,
		Black,
		Gray,
		Red,
		Pink,
		Orange,
		Yellow,
		Green,
		Blue,
		Indigo,
		Violet
	}

	public static class EColorExtensions
	{
		public static Color GetColor(this EColor color)
		{
			switch (color)
			{
				case EColor.Clear:
					return new Color32(0, 0, 0, 0);
				case EColor.White:
					return new Color32(255, 255, 255, 255);
				case EColor.Black:
					return new Color32(0, 0, 0, 255);
				case EColor.Gray:
					return new Color32(128, 128, 128, 255);
				case EColor.Red:
					return new Color32(255, 0, 63, 255);
				case EColor.Pink:
					return new Color32(255, 152, 203, 255);
				case EColor.Orange:
					return new Color32(255, 128, 0, 255);
				case EColor.Yellow:
					return new Color32(255, 211, 0, 255);
				case EColor.Green:
					return new Color32(98, 200, 79, 255);
				case EColor.Blue:
					return new Color32(0, 135, 189, 255);
				case EColor.Indigo:
					return new Color32(75, 0, 130, 255);
				case EColor.Violet:
					return new Color32(128, 0, 255, 255);
				default:
					return new Color32(0, 0, 0, 255);
			}
		}
	}
}
