using UnityEngine;

namespace NaughtyAttributes.Test
{
    public class FolderPathTest : MonoBehaviour
    {
        [FolderPath]
        public string folderPath0;

        [FolderPath("Select Assets folder", "Assets")]
        public string folderPath1;

        public FolderPathNest1 nest1;
    }

    [System.Serializable]
    public class FolderPathNest1
    {
        [FolderPath]
        public string folderPath2;

        public FolderPathNest2 nest2;
    }

    [System.Serializable]
    public struct FolderPathNest2
    {
        [FolderPath]
        public string folderPath3;
    }
}
