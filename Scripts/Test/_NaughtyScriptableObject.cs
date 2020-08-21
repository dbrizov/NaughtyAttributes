using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
	public class _NaughtyScriptableObject : ScriptableObject
	{
		public bool enableMyInt = true;
		public bool showMyFloat = true;

		[EnableIf("enableMyInt")]
		public int myInt;

		[ShowIf("showMyFloat")]
		public float myFloat;

		[MinMaxSlider(0.0f, 1.0f)]
		public Vector2 mySlider;

		//[ReorderableList]
		public List<int> list;

		[Button]
		private void IncrementMyInt()
		{
			myInt++;
		}

		[Button("Decrement My Int")]
		private void DecrementMyInt()
		{
			myInt--;
		}
	}
}
