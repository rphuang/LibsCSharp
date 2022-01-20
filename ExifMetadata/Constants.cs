namespace ExifMetadata
{
    static class Constants
    {
        #region constants for directory name

        public const string JpegDirectoryName = "JPEG";
        public const string ExifSubIfdDirectoryName = "Exif SubIFD";
        public const string ExifIfd0DirectoryName = "Exif IFD0";
        public const string FileDirectoryName = "File";

        #endregion

        #region constants for directory JPEG

        public const string ImageWidthTagName = "Image Width";
        public const string ImageHeightTagName = "Image Height";

        #endregion

        #region constants for directory "Exif SubIFD"

        public const string DateOriginal = "Date/Time Original";

        #endregion

        #region constants for directory File

        public const string FileSizeTagName = "File Size";

        #endregion

        #region constants for directory 
        #endregion

        public static readonly char[] SpaceDelimiter = new char[] { ' ' };
    }
}
