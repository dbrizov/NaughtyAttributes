using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
	public class _NaughtyScriptableObject : ScriptableObject
	{
		[Dropdown("GetVectorValues")]
		public Vector3 vectorValue;

		private DropdownList<Vector3> GetVectorValues()
		{
			return new DropdownList<Vector3>()
			{
				{ "Right", Vector3.right },
				{ "Up", Vector3.up },
				{ "Forward", Vector3.forward }
			};
		}

		[Button]
		private void Log()
		{
			Debug.Log(vectorValue);
		}
	}
}
