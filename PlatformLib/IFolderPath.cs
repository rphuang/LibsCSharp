namespace PlatformLib
{
    /// <summary>
    /// IFolderPath encapsulates platform specifics for getting local folder path
    /// </summary>
    public interface IFolderPath
    {
        /// <summary>
        /// Get full path with the specified folderName
        /// </summary>
        string GetFullFolderPath(string folderName);

        /// <summary>
        /// Get full path with the specified rootFolder and folderName
        /// </summary>
        string GetFullFolderPath(string rootFolder, string folderName);
    }
}
