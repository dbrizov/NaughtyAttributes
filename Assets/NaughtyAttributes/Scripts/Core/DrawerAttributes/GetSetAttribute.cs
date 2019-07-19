using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Trigger a Get and Set calls in a Property from a modifying a Field in the Inspector
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class GetSetAttribute : DrawerAttribute
    {
        public readonly string PropertyName;

        public GetSetAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}
