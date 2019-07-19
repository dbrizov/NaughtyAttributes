using System;

namespace NaughtyAttributes
{
    /// <summary>
    /// Can only select scene Objects, not Assets
    /// </summary>
    /// <param name="logToConsole"> Optional: also log an error message to the console</param>
    /// <code>
    /// 
    /// public MyComponent : MonoBehaviour
    /// {
    ///		[OnlySceneObjects]
    ///		public GameObject SomeSceneObject_1;
    ///    
    ///     [OnlySceneObjects(true)] //This one also prints en error message to the console
    ///		public GameObject SomeSceneObject_2;
    /// }
    /// </code>
    [AttributeUsage(AttributeTargets.All, AllowMultiple = false, Inherited = true)]
    public class OnlySceneObjectsAttribute : DrawerAttribute
    {
        public readonly bool LogToConsole;

        public OnlySceneObjectsAttribute(bool logToConsole = false)
        {
            LogToConsole = logToConsole;
        }
    }
}
