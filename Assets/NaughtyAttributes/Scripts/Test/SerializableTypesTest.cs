using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class SerializableTypesTest : MonoBehaviour
	{
		public ClassType Class;
		public StructType Struct;

		[System.Serializable]
		public class ClassType
		{
			public bool ShowNestedMembers;

			[ShowIf("ShowNestedMembers")]
			public NestedClassType NestedClass;

			[ShowIf("ShowNestedMembers")]
			public NestedStructType NestedStruct;

			[ShowIf("ShowNestedMembers")]
			public List<NestedClassType> NestedClassList;

			[ShowIf("ShowNestedMembers")]
			public List<NestedStructType> NestedStructList;
		}

		[System.Serializable]
		public struct StructType
		{
			public bool ShowNestedMembers;

			[ShowIf("ShowNestedMembers")]
			public NestedClassType NestedClass;

			[ShowIf("ShowNestedMembers")]
			public NestedStructType NestedStruct;

			[ShowIf("ShowNestedMembers")]
			public List<NestedClassType> NestedClassList;

			[ShowIf("ShowNestedMembers")]
			public List<NestedStructType> NestedStructList;
		}

		[System.Serializable]
		public class NestedClassType
		{
			public bool ShowNestedMembers;

			[ShowIf("ShowNestedMembers")]
			public NestedClassType2 NestedClass;

			[ShowIf("ShowNestedMembers")]
			public NestedStructType2 NestedStruct;

			[ShowIf("ShowNestedMembers")]
			public List<NestedClassType2> NestedClassList;

			[ShowIf("ShowNestedMembers")]
			public List<NestedStructType2> NestedStructList;
		}

		[System.Serializable]
		public struct NestedStructType
		{
			public bool ShowNestedMembers;

			[ShowIf("ShowNestedMembers")]
			public NestedClassType2 NestedClass;

			[ShowIf("ShowNestedMembers")]
			public NestedStructType2 NestedStruct;

			[ShowIf("ShowNestedMembers")]
			public List<NestedClassType2> NestedClassList;

			[ShowIf("ShowNestedMembers")]
			public List<NestedStructType2> NestedStructList;
		}

		[System.Serializable]
		public class NestedClassType2
		{
			public bool ShowMembers;

			[ShowIf("ShowMembers")]
			public int Int1;

			[ShowIf("ShowMembers")]
			public float Float2;

			[ShowIf("ShowMembers")]
			public string String3;
		}

		[System.Serializable]
		public struct NestedStructType2
		{
			public bool ShowMembers;

			[ShowIf("ShowMembers")]
			public int Int1;

			[ShowIf("ShowMembers")]
			public float Float2;

			[ShowIf("ShowMembers")]
			public string String3;
		}
	}
}
