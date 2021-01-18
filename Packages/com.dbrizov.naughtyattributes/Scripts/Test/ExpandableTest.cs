using UnityEngine;

namespace NaughtyAttributes.Test
{
	public class ExpandableTest : MonoBehaviour
	{
		[Expandable]
		public ScriptableObject obj0;

		public ExpandableScriptableObjectNest1 nest1;
	}

	[System.Serializable]
	public class ExpandableScriptableObjectNest1
	{
		[Expandable]
		public ScriptableObject obj1;

		public ExpandableScriptableObjectNest2 nest2;
	}

	[System.Serializable]
	public class ExpandableScriptableObjectNest2
	{
		[Expandable]
		public ScriptableObject obj2;
	}
}
