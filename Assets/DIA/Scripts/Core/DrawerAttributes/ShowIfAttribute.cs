using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowIfAttribute : DrawerAttribute
{
    private string predicateName;

    public ShowIfAttribute(string predicateName)
    {
        this.predicateName = predicateName;
    }

    public string PredicateName
    {
        get
        {
            return this.predicateName;
        }
    }
}
