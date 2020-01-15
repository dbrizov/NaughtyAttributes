using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Allows selection of enabled scenes in build settings
    /// </summary>
    [AttributeUsage(AttributeTargets.Field)]
    public class SceneAttribute : DrawerAttribute { }
}
