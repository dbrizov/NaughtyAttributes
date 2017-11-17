using System;

namespace NaughtyAttributes.Editor
{
    public abstract class PropertyGrouper
    {
        public abstract void BeginGroup(string label);

        public abstract void EndGroup();
    }
}
