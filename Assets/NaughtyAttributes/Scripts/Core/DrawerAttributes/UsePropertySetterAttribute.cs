namespace NaughtyAttributes
{
    /// <summary>
    /// Use this on a private serialized member that is exposed via a public property. 
    /// When setting its value in the inspector, it will be done via the property setter instead of skipping it.
    /// <para>If the property name matches the name displayed on the inspector without spaces, the name parameter can be omitted.</para>
    /// </summary>
    public class UsePropertySetterAttribute : DrawerAttribute
    {
        public readonly string propertyName;
        public readonly bool autoFindProperty;

        public UsePropertySetterAttribute()
        {
            propertyName = null;
            autoFindProperty = true;
        }

        public UsePropertySetterAttribute(string propertyName)
        {
            this.propertyName = propertyName;
            autoFindProperty = false;
        }
    }
}