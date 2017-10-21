using System;

namespace NaughtyAttributes.Editor
{
    public interface IAttribute
    {
        Type TargetAttributeType { get; }
    }
}
