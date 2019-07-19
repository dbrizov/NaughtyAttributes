using System;

namespace NaughtyAttributes
{

    /// <summary>
    /// Can only select assets in the project, not objects in the scene
    /// </summary>
    /// <param name="logToConsole"> Optional: also log an error message to the console</param>
    /// <code>
    /// 
    /// public MyComponent : MonoBehaviour
    /// {
    ///		[OnlyAssets]
    ///		public GameObject SomePrefab_1;
    ///    
    ///     [OnlyAssets(true)] //This one also prints en error message to the console
    ///		public GameObject SomePrefab_2;
    /// }
    /// </code>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class OnlyAssetsAttribute : DrawerAttribute
    {
        public readonly bool LogToConsole;


        public OnlyAssetsAttribute(bool logToConsole = false)
        {
            LogToConsole = logToConsole;
        }
    }
}
