using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	//[CreateAssetMenu(fileName = "NaughtyScriptableObject", menuName = "NaughtyAttributes/_NaughtyScriptableObject")]
	public class _NaughtyScriptableObject : ScriptableObject
	{
		[Expandable]
		public List<_TestScriptableObject> list;

#if UNITY_EDITOR
		public UnityEditor.Animations.AnimatorController animatorController0;
#endif
		
		[AnimatorParam("animatorController0")]
		public int hashController0;

		[AnimatorParam("animatorController0")]
		public string nameController0;
		
		
		[Button("Log 'hashController0' and 'nameController0'")]
		private void TestLog()
		{
			Debug.Log($"hashController0 = {hashController0}");
			Debug.Log($"nameController0 = {nameController0}");
			Debug.Log($"Animator.StringToHash(nameController0) = {Animator.StringToHash(nameController0)}");
		}
	}
}
