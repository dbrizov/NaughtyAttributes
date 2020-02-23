using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class _NaughtyComponent : MonoBehaviour
	{
		public bool flag0 = true;
		public bool flag1 = true;

		[BoxGroup("Sliders")]
		[MinMaxSlider(0, 1)]
		[ShowIf("flag0")]
		public Vector2 slider0;

		[BoxGroup("Sliders")]
		[MinMaxSlider(0, 1)]
		[ShowIf("flag1")]
		public Vector2 slider1;
	}

	//[System.Serializable]
	//public class MyClass
	//{
	//	public string level1;

	//	public MyStruct myStruct;
	//}

	//[System.Serializable]
	//public struct MyStruct
	//{
	//	[ResizableTextArea]
	//	public string level2;
	//}
}
