using MetadataExtractor;
using System;
using System.Collections.Generic;

namespace ExifMetadata
{
    /// <summary>
    /// factory class uses MetadataExtractor to get metadata
    /// </summary>
    public class MetadataExtractorProvider
    {
        /// <summary>
        /// sample usage code to extract EXIF metadata from a file and create Metadata object
        /// </summary>
        public static Metadata GetMetadata(string filePath)
        {
            MetadataExtractorProvider extractor = new MetadataExtractorProvider();
            extractor.AddMap(MetadataExtractorProvider.StandardTagMapDefs);
            // add more custom mapping

            return new Metadata(extractor.Extract(filePath));
        }

        /// <summary>
        /// construct with empty mapping
        /// </summary>
        public MetadataExtractorProvider()
        {
        }

        /// <summary>
        /// add additional tag mapping to extract
        /// </summary>
        /// <param name="maps"></param>
        public void AddMap(IEnumerable<TagMapDef> maps)
        {
            _tagMapDefs.AddRange(maps);
        }

        /// <summary>
        /// Get metadata from file 
        /// </summary>
        /// <param name="filePath"></param>
        public Dictionary<string, object> Extract(string filePath)
        {
            try
            {
                IEnumerable<Directory> directories = ImageMetadataReader.ReadMetadata(filePath);
                _directories = new Dictionary<string, List<Directory>>();
                foreach (var directory in directories)
                {
                    List<Directory> dir;
                    if (!_directories.TryGetValue(directory.Name, out dir))
                    {
                        dir = new List<Directory>();
                        _directories.Add(directory.Name, dir);
                    }
                    dir.Add(directory);
                }
                return ExtractTags();
            }
            catch (Exception err)
            {
                // todo:
                //Log.Warn("Error getting metadata from file {0}: {1}", filePath, err.ToString());
            }
            return null;
        }

        protected Dictionary<string, object> ExtractTags()
        {
            Dictionary<string, object> keyValuePairs = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            foreach (TagMapDef def in StandardTagMapDefs)
            {
                try
                {
                    string tagValue = GetTag(def.DirectoryName, def.TagName);
                    if (tagValue != null) keyValuePairs.Add(def.Name, tagValue);
                }
                catch (Exception err)
                {
                    // todo
                    //Log.Warn("Error getting tag {0}: {1}", def.Name, err.ToString());
                }
            }

            return keyValuePairs;
        }

        protected int GetIntTag(string directoryName, string tagName, int defaultValue = 0)
        {
            string tag = GetTag(directoryName, tagName);
            if (string.IsNullOrEmpty(tag)) return defaultValue;
            return ExifUtil.GetInt(tag);
        }

        protected DateTime? GetDateTimeTag(string directoryName, string tagName, string format)
        {
            string tag = GetTag(directoryName, tagName);
            return ExifUtil.GetDateTime(tag, format);
        }

        protected string GetTag(string directoryName, string tagName)
        {
            List<Directory> directories = _directories[directoryName];
            if (directories == null) return null;
            foreach (Directory directory in directories)
            {
                string tag = ExifUtil.GetTag(directory, tagName);
                if (tag != null) return tag;
            }
            return null;
        }

        /// <summary>
        /// definition for mapping from MetadataExtractor's (directoryName, tagName) to standard tags in TagNames
        /// </summary>
        public class TagMapDef
        {
            /// <summary>
            /// constructor
            /// </summary>
            public TagMapDef(string name, string directoryName, string tagName)
            {
                Name=name;
                DirectoryName=directoryName;
                TagName=tagName;
            }

            /// <summary>
            /// standard tag name defined in TagNames
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// the directory name in MetadataExtractor
            /// </summary>
            public string DirectoryName { get; set; }
            /// <summary>
            /// the tag name in MetadataExtractor
            /// </summary>
            public string TagName { get; set; }
        }

        /// <summary>
        /// load tag mapping from cav file in format: "Name,DirectoryName,TagName"
        /// </summary>
        public static IEnumerable<TagMapDef> LoadTagMapFromCsv(string filePath)
        {
            if (string.IsNullOrEmpty(filePath)) return null;
            List<TagMapDef> tags = new List<TagMapDef>();
            try
            {
                using (System.IO.TextReader reader = System.IO.File.OpenText(filePath))
                {
                    string buffer;
                    while ((buffer = reader.ReadLine()) != null)
                    {
                        buffer = buffer.Trim();
                        if (buffer.Length == 0 || buffer.StartsWith("#")) continue;

                        string[] parts = buffer.Split(CommaDelimiter);
                        if (parts.Length == 3)
                        {
                            tags.Add(new TagMapDef(parts[0], parts[1], parts[2]));
                        }
                        else
                        {
                            // todo:
                            //Log.Warn("Invalid tag mapping from file {0}: {1}", filePath, buffer);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                // todo
            }
            return tags;
        }

        /// <summary>
        /// StandardTagMapDefs defines the mapping for standard tags provided by MetadataExtractor
        /// </summary>
        public static TagMapDef[] StandardTagMapDefs = 
        {
            // file related tags
            new TagMapDef(TagNames.FileName, "File", "File Name"),
            new TagMapDef(TagNames.FileSize, "File", "File Size"),
            new TagMapDef(TagNames.FileModifiedDate, "File", "File Modified Date"),
            new TagMapDef(TagNames.FileType, "File Type", "Detected File Type Name"),
            new TagMapDef(TagNames.MIMEType, "File Type", "Detected MIME Type"),

            // camera/software related tags
            new TagMapDef(TagNames.Make, "Exif IFD0", "Make"),
            new TagMapDef(TagNames.CameraModel, "Exif IFD0", "Model"),
            new TagMapDef(TagNames.Software, "Exif IFD0", "Software"),
            new TagMapDef(TagNames.Lens, "Exif SubIFD", "Lens Make"),
            new TagMapDef(TagNames.LensModel, "Exif SubIFD", "Lens Model"),

            // location tags
            new TagMapDef(TagNames.Latitude, "GPS", "GPS Latitude"),
            new TagMapDef(TagNames.LatitudeRef, "GPS", "GPS Latitude Ref"),
            new TagMapDef(TagNames.Longitude, "GPS", "GPS Longitude"),
            new TagMapDef(TagNames.LongitudeRef, "GPS", "GPS Longitude Ref"),
            new TagMapDef(TagNames.Altitude, "GPS", "GPS Altitude"),
            new TagMapDef(TagNames.AltitudeRef, "GPS", "GPS Altitude Ref"),
            new TagMapDef(TagNames.Direction, "GPS", "GPS Img Direction"),
            new TagMapDef(TagNames.DirectionRef, "GPS", "GPS Img Direction Ref"),
            new TagMapDef(TagNames.GpsDateStamp, "GPS", "GPS Date Stamp"),
            new TagMapDef(TagNames.GpsTimeStamp, "GPS", "GPS Time-Stamp"),

            // image tags
            new TagMapDef(TagNames.DateTaken, "Exif SubIFD", "Date/Time Original"),
            new TagMapDef(TagNames.ImageHeight, "JPEG", "Image Height"),
            new TagMapDef(TagNames.ImageWidth, "JPEG", "Image Width"),
            new TagMapDef(TagNames.DataPrecision, "JPEG", "Data Precision"),
            new TagMapDef(TagNames.Orientation, "Exif IFD0", "Orientation"),
            new TagMapDef(TagNames.XResolution, "Exif IFD0", "X Resolution"),
            new TagMapDef(TagNames.YResolution, "Exif IFD0", "Y Resolution"),
            new TagMapDef(TagNames.ResolutionUnit, "Exif IFD0", "Resolution Unit"),
            new TagMapDef(TagNames.ExposureTime, "Exif SubIFD", "Exposure Time"),
            new TagMapDef(TagNames.ExposureMode, "Exif SubIFD", "Exposure Mode"),
            new TagMapDef(TagNames.ExposureCompensation, "Exif SubIFD", "Exposure Bias Value"),
            new TagMapDef(TagNames.ExposureProgram, "Exif SubIFD", "Exposure Program"),
            new TagMapDef(TagNames.FNumber, "Exif SubIFD", "F-Number"),
            new TagMapDef(TagNames.ISO, "Exif SubIFD", "ISO Speed Ratings"),
            new TagMapDef(TagNames.MeteringMode, "Exif SubIFD", "Metering Mode"),
            new TagMapDef(TagNames.WhiteBalance, "Exif SubIFD", "White Balance"),
            new TagMapDef(TagNames.WhiteBalanceMode, "Exif SubIFD", "White Balance Mode"),
            new TagMapDef(TagNames.Flash, "Exif SubIFD", "Flash"),
            new TagMapDef(TagNames.FocalLength, "Exif SubIFD", "Focal Length"),
            new TagMapDef(TagNames.FocalLengthIn35mm, "Exif SubIFD", "Focal Length 35"),
            new TagMapDef(TagNames.Comment, "Exif SubIFD", "User Comment"),
            new TagMapDef(TagNames.SensorMode, "Exif SubIFD", "Sensing Method"),
            new TagMapDef(TagNames.DigitalZoom, "Exif SubIFD", "Digital Zoom Ratio"),
            new TagMapDef(TagNames.SceneCaptureType, "Exif SubIFD", "Scene Capture Type"),
            new TagMapDef(TagNames.Contrast, "Exif SubIFD", "Contrast"),
            new TagMapDef(TagNames.Saturation, "Exif SubIFD", "Saturation"),
            new TagMapDef(TagNames.Sharpness, "Exif SubIFD", "Sharpness"),
            new TagMapDef(TagNames.SubjectDistanceRange, "Exif SubIFD", "Subject Distance Range"),
        };

        protected static readonly char[] CommaDelimiter = { ',' };

        // member to store information from MetadataExtractor
        protected Dictionary<string, List<Directory>> _directories;

        protected List<TagMapDef> _tagMapDefs = new List<TagMapDef>();
    }
}
