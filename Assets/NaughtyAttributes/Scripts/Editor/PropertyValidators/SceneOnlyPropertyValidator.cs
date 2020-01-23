using UnityEngine;
using UnityEditor;

namespace NaughtyAttributes.Editor
{
    public class SceneOnlyPropertyValidator : AssetsOnlyPropertyValidator
    {
        protected override bool IsValid(SerializedProperty property)
        {
            return !base.IsValid(property);
        }
    }
}
