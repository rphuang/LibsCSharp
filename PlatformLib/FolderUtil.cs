using System.IO;
using Xamarin.Forms;

namespace PlatformLib
{
    public static class FolderUtil
    {
        /// <summary>
        /// Get root folder path (such as docs, dcim, downloads, ...)
        /// </summary>
        public static string GetFolderPath(string rootFolder, bool createFolder)
        {
            string folderPath = DependencyService.Get<IFolderPath>()?.GetFullFolderPath(rootFolder);
            // create the folder if not there
            if (createFolder && !Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        /// <summary>
        /// Get root folder path (such as docs, dcim, downloads, ...)
        /// </summary>
        public static string GetFolderPath(string rootFolder, string folderName, bool createFolder)
        {
            string rootFolderPath = DependencyService.Get<IFolderPath>()?.GetFullFolderPath(rootFolder);
            string folderPath = Path.Combine(rootFolderPath, folderName);
            // create the folder if not there
            if (createFolder && !Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);
            return folderPath;
        }

        /// <summary>
        /// Get folder path for "docs"
        /// </summary>
        public static string GetDocsFolderPath(bool createFolder)
        {
            return GetFolderPath("docs", createFolder);
        }
    }
}
