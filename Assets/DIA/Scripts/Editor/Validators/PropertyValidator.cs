using UnityEditor;

public abstract class PropertyValidator
{
    public void ValidateProperty(SerializedProperty property)
    {
        this.ValidatePropertyImplementation(property);
    }

    protected abstract void ValidatePropertyImplementation(SerializedProperty property);
}