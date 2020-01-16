using UnityEditor;

namespace NaughtyAttributes.Editor
{
	[CustomPropertyDrawer(typeof(DisableIfAttribute))]
	public class DisableIfPropertyDrawer : EnableIfPropertyDrawer
	{
	}
}
