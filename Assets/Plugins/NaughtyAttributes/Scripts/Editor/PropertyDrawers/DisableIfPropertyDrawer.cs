using UnityEditor;

namespace NaughtyAttributes.Editor
{
    [PropertyDrawer(typeof(DisableIfAttribute))]
    public class DisableIfPropertyDrawer : EnableIfPropertyDrawer
    {
    }
}
