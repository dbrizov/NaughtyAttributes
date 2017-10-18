using UnityEditor;
using System;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyGrouper
    {
        public abstract void GroupProperties(SerializedProperty[] properties, Action<SerializedProperty> drawPropertyFunc);
    }
}
