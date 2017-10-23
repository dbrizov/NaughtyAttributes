using UnityEngine;
using System.IO;

namespace NaughtyAttributes.Editor
{
    public static class IOUtility
    {
        public static string GetPersistentDataPath()
        {
            return Application.persistentDataPath + "/";
        }

        public static void WriteToFile(string filePath, string content)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            using (fileStream)
            {
                StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.ASCII);
                using (streamWriter)
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

        public static string ReadFromFile(string filePath)
        {
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            using (fileStream)
            {
                StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.ASCII);
                using (streamReader)
                {
                    string content = streamReader.ReadToEnd();
                    return content;
                }
            }
        }

        public static bool FileExists(string filePath)
        {
            return File.Exists(filePath);
        }

        public static string GetPathRelativeToProjectFolder(string fullPath)
        {
            int indexOfAssetsWord = fullPath.IndexOf("\\Assets");
            string relativePath = fullPath.Substring(indexOfAssetsWord + 1);

            return relativePath;
        }
    }
}
