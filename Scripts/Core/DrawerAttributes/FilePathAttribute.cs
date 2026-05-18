using System;

namespace NaughtyAttributes
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class FilePathAttribute : DrawerAttribute
    {
        public string Title { get; private set; }
        public string Directory { get; private set; }
        public string Filter { get; private set; }
        public bool RelativePath { get; private set; }
        public bool ValidateExists { get; private set; }

        public FilePathAttribute(string title = "Select file", string directory = "", string filter = "", bool relativePath = false, bool validateExists = false)
        {
            Title = title;
            Directory = directory;
            Filter = filter;
            RelativePath = relativePath;
            ValidateExists = validateExists;
        }
    }
}
