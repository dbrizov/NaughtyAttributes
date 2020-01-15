using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public interface ISpecialCasePropertyDrawer
	{
		void OnGUI(SerializedProperty property);
	}

	public static class SpecialCaseDrawerAttributeExtensions
	{
		public static ISpecialCasePropertyDrawer GetDrawer(this ISpecialCaseDrawerAttribute attr)
		{
			if (attr.GetType() == typeof(ReorderableListAttribute))
			{
				return ReorderableListPropertyDrawer.Instance;
			}
			else
			{
				return null;
			}
		}
	}
}
