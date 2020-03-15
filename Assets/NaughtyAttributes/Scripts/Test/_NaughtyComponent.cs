using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		public bool show1;
		public bool show2;

		[ShowIf(EConditionOperator.And, "show1", "show2")]
		public int[] showIfAll;

		[ShowIf(EConditionOperator.Or, "show1", "show2")]
		public int[] showIfAny;

		public MyClass nest1;
	}

	[System.Serializable]
	public class MyClass
	{
		public bool show1;
		public bool show2;
		public bool Show1 { get { return show1; } }
		public bool Show2 { get { return show2; } }

		[ShowIf(EConditionOperator.And, "Show1", "Show2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int[] showIfAll;

		[ShowIf(EConditionOperator.Or, "Show1", "Show2")]
		[AllowNesting] // Because it's nested we need to explicitly allow nesting
		public int[] showIfAny;
	}

	//[System.Serializable]
	//public struct MyStruct
	//{
	//	[ResizableTextArea]
	//	public string level2;
	//}
}
