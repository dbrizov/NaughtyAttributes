using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class FilePathTest : MonoBehaviour
    {
        [FilePath]
        public string filePath0;

        [FilePath("Select a C# file", "", "cs")]
        public string filePath1;

        [FilePath("Select a relative file", "", "", relativePath: true)]
        public string relativeFilePath;

        [FilePath("Select an existing file", "", "", validateExists: true)]
        public string existingFilePath;

        [FilePath("Select an existing relative file", "", "", relativePath: true, validateExists: true)]
        public string existingRelativeFilePath;

        [FolderPath("Select a relative folder", "", relativePath: true)]
        public string relativeFolderPath;

        public FilePathNest1 nest1;
    }

    [System.Serializable]
    public class FilePathNest1
    {
        [FilePath("Select a text file", "", "txt")]
        public string filePath2;

        public FilePathNest2 nest2;
    }

    [System.Serializable]
    public struct FilePathNest2
    {
        [FilePath]
        public string filePath3;
    }
}
