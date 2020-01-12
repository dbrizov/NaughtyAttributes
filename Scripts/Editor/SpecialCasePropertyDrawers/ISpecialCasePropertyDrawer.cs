using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public interface ISpecialCasePropertyDrawer
	{
		void OnGUI_Custom(SerializedProperty property);
		void ClearCache();
	}
}
