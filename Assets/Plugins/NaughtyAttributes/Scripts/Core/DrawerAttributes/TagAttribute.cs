using UnityEngine;

namespace NaughtyAttributes
{
    /// <summary>
    /// Make tags appear as tag popup fields 
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = false)]
    public class TagAttribute : DrawerAttribute
    {
    }
}
