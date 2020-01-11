using UnityEditor;

namespace NaughtyAttributes.Editor
{
	public class NaughtyPropertyDrawer : PropertyDrawer
	{
		public UnityEngine.Object GetTargetObject(SerializedProperty property)
		{
			return property.serializedObject.targetObject;
		}
	}
}
