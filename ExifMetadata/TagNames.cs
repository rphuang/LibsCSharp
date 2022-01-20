using System;
using System.Collections.Generic;
using System.Text;

namespace ExifMetadata
{
    /// <summary>
    /// TagNames defines the standard EXIF tag names
    /// </summary>
    public static class TagNames
    {
        // file related tags
        public const string FileName = "File Name";
        public const string FileSize = "File Size";
        public const string FileType = "File Type";
        public const string FileDirectory = "Directory";
        public const string MIMEType = "MIME Type";
        public const string FileModifiedDate = "File Modified Date";

        // camera/software related tags
        public const string Make = "Make";
        public const string CameraModel = "Camera Model";
        public const string FirmwareVersion = "Firmware Version";
        public const string Lens = "Lens";
        public const string LensModel = "Lens Model";
        public const string Software = "Software";
        public const string ScaleFactorTo35mmEquivalent = "Scale Factor To 35mm";

        // image related tags
        public const string DateImported = "Date Imported";
        public const string DateModified = "Date Modified";
        public const string DateTaken = "Date Taken";
        public const string CreateDate = "Create Date";
        public const string ModifyDate = "Modify Date";
        public const string FocalLength = "Focal Length";
        public const string FocalLengthIn35mm = "Focal Length In 35mm";
        //public const string FocalLength_1 = "Focal Length (1)";
        public const string Aperture = "Aperture";
        public const string FNumber = "F Number";
        public const string ShutterSpeed = "Shutter Speed";
        public const string ExposureTime = "Exposure Time";
        public const string ExposureMode = "Exposure Mode";
        public const string ExposureProgram = "Exposure Program";
        public const string ExposureCompensation = "Exposure Compensation";
        public const string Flash = "Flash";
        public const string FlashMode = "Flash Mode";
        public const string FlashSync = "Flash Sync";
        public const string MeteringMode = "Metering Mode";
        public const string WhiteBalance = "White Balance";
        public const string WhiteBalanceMode = "White Balance Mode";
        public const string WhiteBalanceFine = "White Balance Fine";
        public const string ISO = "ISO";
        public const string ISOSetting = "ISO Setting";
        public const string FocusMode = "Focus Mode";
        public const string AFPointsInFocus = "AF Points In Focus";
        public const string Contrast = "Contrast";
        public const string Saturation = "Saturation";
        public const string Sharpness = "Sharpness";
        public const string NoiseReduction = "Noise Reduction";
        public const string ImageSize = "Image Size";
        public const string ImageWidth = "Image Width";
        public const string ImageHeight = "Image Height";
        public const string FacesDetected = "Faces Detected";
        public const string ExifImageWidth = "Exif Image Width";
        public const string ExifImageLength = "Exif Image Length";
        public const string BitsPerSample = "Bits Per Sample";
        public const string CircleOfConfusion = "Circle Of Confusion";
        public const string HyperfocalDistance = "Hyperfocal Distance";
        public const string LightValue = "Light Value";
        public const string Orientation = "Orientation";
        public const string ResolutionUnit = "Resolution Unit";
        public const string ColorSpace = "Color Space";
        public const string SensingMethod = "Sensing Method";
        public const string SubjectDistanceRange = "Subject Distance Range";
        public const string Compression = "Compression";
        public const string XResolution = "X Resolution";
        public const string YResolution = "Y Resolution";
        public const string ShutterCount = "Shutter Count";
        public const string CustomRendered = "Custom Rendered";
        public const string SceneCaptureType = "Scene Capture Type";
        public const string Quality = "Quality";
        public const string DriveMode = "Drive Mode";
        public const string DataPrecision = "Data Precision";
        public const string DigitalZoom = "Digital Zoom";
        public const string SensorMode = "Sensor Mode";
        public const string SequenceNumber = "Sequence Number";
        public const string AFType = "AF Type";

        // location tags
        public const string Latitude = "GPS Latitude";
        public const string LatitudeRef = "GPS Latitude Ref";
        public const string Longitude = "GPS Longitude";
        public const string LongitudeRef = "GPS Longitude Ref";
        public const string Altitude = "GPS Altitude";
        public const string AltitudeRef = "GPS Altitude Ref";
        public const string Direction = "GPS Direction";
        public const string DirectionRef = "GPS Direction Ref";
        public const string GpsDateStamp = "GPS Date Stamp";
        public const string GpsTimeStamp = "GPS Time-Stamp";

        // user tags - rating and keywords
        public const string Rating = "Rating";
        public const string Comment = "Comment";
        public const string Keywords = "Keywords";

        // others
        public const string ThumbnailOffset = "Thumbnail Offset";
        public const string ThumbnailLength = "Thumbnail Length";
        public const string ThumbnailImage = "Thumbnail Image";
        public const string JFIFVersion = "JFIF Version";
        public const string NEFCurve1 = "NEF Curve 1";
        public const string NEFCurve2 = "NEF Curve 2";
        public const string JpgFromRaw = "Jpg From Raw";
        public const string PreviewImage = "Preview Image";
        public const string PreviewImageSize = "Preview Image Size";
        public const string PreviewImageStart = "Preview Image Start";
        public const string PreviewImageLength = "Preview Image Length";
        public const string RedToneReproductionCurve = "Red Tone Reproduction Curve";
        public const string GreenToneReproductionCurve = "Green Tone Reproduction Curve";
        public const string BlueToneReproductionCurve = "Blue Tone Reproduction Curve";
        public const string WorldTimeLocation = "World Time Location";
        public const string HometownCity = "Hometown City";
        public const string DestinationCity = "Destination City";
        public const string HometownDST = "Hometown DST";
        public const string DestinationDST = "Destination DST";
    }
}
