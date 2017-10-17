using System;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
public class PropertyDrawerAttribute : Attribute
{
    private Type targetAttributeType;

    public PropertyDrawerAttribute(Type targetAttributeType)
    {
        this.targetAttributeType = targetAttributeType;
    }

    public Type TargetAttributeType
    {
        get
        {
            return this.targetAttributeType;
        }
    }
}