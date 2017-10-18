using System;

[AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
public class ShowIfAttribute : DrawerAttribute
{
    private string conditionName;

    public ShowIfAttribute(string conditionName)
    {
        this.conditionName = conditionName;
    }

    public string ConditionName
    {
        get
        {
            return this.conditionName;
        }
    }
}
