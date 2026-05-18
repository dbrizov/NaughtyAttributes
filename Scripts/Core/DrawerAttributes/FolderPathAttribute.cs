using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FolderPathAttribute : DrawerAttribute
    {
        public string Title { get; private set; }
        public string DefaultPath { get; private set; }
        public bool RelativePath { get; private set; }

        public FolderPathAttribute(string title = "Select folder", string defaultPath = "", bool relativePath = false)
        {
            Title = title;
            DefaultPath = defaultPath;
            RelativePath = relativePath;
        }
    }
}
