using System;
using System.IO;
using UnityEngine;

namespace NaughtyAttributes.Editor
{
    public static class NaughtyPathUtility
    {
        public static string GetProjectRelativePath(string path)
        {
            string projectPath = GetProjectPathWithTrailingSeparator();
            string fullPath = Path.GetFullPath(path);

            return fullPath.StartsWith(projectPath, StringComparison.OrdinalIgnoreCase)
                ? NormalizePath(fullPath.Substring(projectPath.Length))
                : NormalizePath(path);
        }

        public static string GetProjectAbsolutePath(string path)
        {
            if (string.IsNullOrEmpty(path) || Path.IsPathRooted(path))
            {
                return path;
            }

            string projectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            return Path.GetFullPath(Path.Combine(projectPath, path));
        }

        public static string NormalizePath(string path)
        {
            return path.Replace(Path.DirectorySeparatorChar, '/').Replace(Path.AltDirectorySeparatorChar, '/');
        }

        private static string GetProjectPathWithTrailingSeparator()
        {
            string projectPath = Path.GetFullPath(Path.Combine(Application.dataPath, ".."));
            if (!projectPath.EndsWith(Path.DirectorySeparatorChar.ToString()))
            {
                projectPath += Path.DirectorySeparatorChar;
            }

            return projectPath;
        }
    }
}
