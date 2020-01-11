namespace NaughtyAttributes
{
    /// <summary>
    /// Use this on a serialized field that is exposed through a property. 
    /// When setting its value in the inspector, the setter will be used.
    /// <para>If the property name matches the field's inspector display name, the name doesn't have to be provided.</para>
    /// <para>[Delayed] use is recommended, as it avoids spamming the setter with half-written values.</para>
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