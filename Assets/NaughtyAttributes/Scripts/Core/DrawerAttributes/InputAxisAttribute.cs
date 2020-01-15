using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Allows selection of input axes
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class InputAxisAttribute : DrawerAttribute { }
}
