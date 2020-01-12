using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public abstract class SpecialCasePropertyDrawer
	{
		public abstract void OnGUI(SerializedProperty property);
		public abstract void ClearCache();
	}
}
