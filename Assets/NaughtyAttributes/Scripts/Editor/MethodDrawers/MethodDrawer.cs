using System.Reflection;

namespace NaughtyAttributes.Editor
{
    public abstract class MethodDrawer
    {
        public abstract void DrawMethod(System.Object target, MethodInfo methodInfo);

        public virtual void ClearCache()
        {

        }
    }
}
