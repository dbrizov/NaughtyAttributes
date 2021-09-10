﻿using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class AnimatorParamTest : MonoBehaviour
	{
		public Animator animator0;

#if UNITY_EDITOR
		public UnityEditor.Animations.AnimatorController animatorController0;
#endif

		[AnimatorParam("animator0")]
		public int hash0;
		[AnimatorParam("animator0")]
		public string name0;
		
		[AnimatorParam("animatorController0")]
		public int hashController0;

		[AnimatorParam("animatorController0")]
		public string nameController0;

		public AnimatorParamNest1 nest1;

		[Button("Log 'hash0', 'hashController0' and 'name0', 'nameController0'")]
		private void TestLog()
		{
			Debug.Log($"hash0 = {hash0}");
			Debug.Log($"name0 = {name0}");
			Debug.Log($"Animator.StringToHash(name0) = {Animator.StringToHash(name0)}");
			
			Debug.Log($"hashController0 = {hashController0}");
			Debug.Log($"nameController0 = {nameController0}");
			Debug.Log($"Animator.StringToHash(nameController0) = {Animator.StringToHash(nameController0)}");
		}
	}

	[System.Serializable]
	public class AnimatorParamNest1
	{
		public Animator animator1;
		
#if UNITY_EDITOR
		public UnityEditor.Animations.AnimatorController animatorController1;
#endif
		
		private Animator Animator1 => animator1;
		
#if UNITY_EDITOR
		private UnityEditor.Animations.AnimatorController AnimatorController1 => animatorController1;
#endif
		
		[AnimatorParam("Animator1", AnimatorControllerParameterType.Bool)]
		public int hash1;

		[AnimatorParam("Animator1", AnimatorControllerParameterType.Float)]
		public string name1;
		
		[AnimatorParam("AnimatorController1", AnimatorControllerParameterType.Bool)]
		public int hashController1;

		[AnimatorParam("AnimatorController1", AnimatorControllerParameterType.Float)]
		public string nameController1;
		
		public AnimatorParamNest2 nest2;
	}

	[System.Serializable]
	public class AnimatorParamNest2
	{
		public Animator animator2;
		
#if UNITY_EDITOR
		public UnityEditor.Animations.AnimatorController animatorController2;
#endif
		
		private Animator GetAnimator2() => animator2;
		
#if UNITY_EDITOR
		private UnityEditor.Animations.AnimatorController GetAnimatorController2() => animatorController2;
#endif
		
		[AnimatorParam("GetAnimator2", AnimatorControllerParameterType.Int)]
		public int hash1;

		[AnimatorParam("GetAnimator2", AnimatorControllerParameterType.Trigger)]
		public string name1;
		
		[AnimatorParam("GetAnimatorController2", AnimatorControllerParameterType.Int)]
		public int hashController2;

		[AnimatorParam("GetAnimatorController2", AnimatorControllerParameterType.Trigger)]
		public string nameController2;
	}
}
