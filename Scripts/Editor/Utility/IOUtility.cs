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
            using (FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fileStream, System.Text.Encoding.ASCII))
                {
                    streamWriter.WriteLine(content);
                }
            }
        }

        public static string ReadFromFile(string filePath)
        {
            using (FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                using (StreamReader streamReader = new StreamReader(fileStream, System.Text.Encoding.ASCII))
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
