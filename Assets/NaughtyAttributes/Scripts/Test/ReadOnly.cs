using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ReadOnly : MonoBehaviour
	{
		[ReadOnly]
		public int readOnlyInt = 5;
	}
}
