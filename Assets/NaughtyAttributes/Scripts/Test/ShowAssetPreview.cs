using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ShowAssetPreview : MonoBehaviour
	{
		[ShowAssetPreview]
		public Sprite sprite;

		[ShowAssetPreview(96, 96)]
		public GameObject prefab;
	}
}
