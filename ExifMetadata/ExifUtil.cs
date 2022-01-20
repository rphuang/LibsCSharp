using MetadataExtractor;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace ExifMetadata
{
    /// <summary>
    /// utility class for Exif metadata
    /// </summary>
    public class ExifUtil
    {
        /// <summary>
        /// Get image size for specified photo file path
        /// </summary>
        public static void GetImageSize(string filePath, out int width, out int height)
        {
            IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            Directory directory = GetDirectory(directories, Constants.JpegDirectoryName);
            width = GetintTag(directory, Constants.ImageWidthTagName);
            height = GetintTag(directory, Constants.ImageHeightTagName);
        }

        /// <summary>
        /// Get image size and dateTaken for specified photo file path
        /// </summary>
        public static void GetImageMetadata(string filePath, out int width, out int height, out DateTime? dateTaken, string format)
        {
            IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
            Directory directory = GetDirectory(directories, Constants.JpegDirectoryName);
            width = GetintTag(directory, Constants.ImageWidthTagName);
            height = GetintTag(directory, Constants.ImageHeightTagName);
            directory = GetDirectory(directories, Constants.ExifSubIfdDirectoryName);
            dateTaken = GetDateTimeTag(directory, Constants.DateOriginal, format);
        }

        internal static int GetintTag(Directory directory, string tagName)
        {
            string strValue = GetTag(directory, tagName);
            return GetInt(strValue);
        }

        static char[] DatetimeDelimiter = { ':' };
        internal static DateTime? GetDateTimeTag(Directory directory, string tagName, string format)
        {
            string strValue = GetTag(directory, tagName);
            return GetDateTime(strValue, format);
        }

        internal static int GetInt(string tagValue)
        {
            if (string.IsNullOrEmpty(tagValue)) return 0;
            string[] parts = tagValue.Trim().Split(Constants.SpaceDelimiter);
            return int.Parse(parts[0]);
        }

        internal static DateTime? GetDateTime(string tagValue, string format)
        {
            if (string.IsNullOrEmpty(tagValue)) return null;
            try
            {
                return DateTime.ParseExact(tagValue, format, CultureInfo.InvariantCulture);
            }
            catch (Exception err)
            {
            }
            return null;
        }

        internal static string GetTag(Directory directory, string tagName)
        {
            if (directory == null || string.IsNullOrEmpty(tagName)) return null;

            foreach (Tag tag in directory.Tags)
            {
                if (string.Equals(tagName, tag.Name, StringComparison.OrdinalIgnoreCase)) return tag.Description;
            }
            return null;
        }

        internal static Directory GetDirectory(IEnumerable<Directory> directories, string directoryName)
        {
            if (directories == null || string.IsNullOrEmpty(directoryName)) return null;

            foreach (var directory in directories)
            {
                if (string.Equals(directoryName, directory.Name, StringComparison.OrdinalIgnoreCase)) return directory;
            }
            return null;
        }
    }
}
