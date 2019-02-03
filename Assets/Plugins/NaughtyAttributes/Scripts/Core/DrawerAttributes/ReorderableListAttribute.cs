using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class ReorderableListAttribute : DrawerAttribute
    {
        public bool paginate;
        public int pageSize;
        public ReorderableListAttribute(bool _paginate = false,int _pageSize = 3)
        {
            paginate = _paginate;
            pageSize = _pageSize;
        }
    }
}
