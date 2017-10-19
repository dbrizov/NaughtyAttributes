using System;

namespace NaughtyAttributes.Editor
{
    public interface IPropertyAttribute
    {
        Type TargetAttributeType { get; }
    }
}
