using System;
using System.Collections.Generic;

namespace ExifMetadata
{
    /// <summary>
    /// the base class for media file's metadata
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// constructor
        /// </summary>
        public Metadata()
        {
            _properties = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// constructor
        /// </summary>
        public Metadata(Dictionary<string, object> properties)
        {
            _properties = properties;
        }

        /// <summary>
        /// get tag value by tag name
        /// </summary>
        public T GetTagValue<T>(string tagName)
        {
            return (T)_properties[tagName];
        }

        /// <summary>
        /// get tag value by tag name
        /// </summary>
        public bool TryGetTagValue<T>(string tagName, out T value)
        {
            value = default;
            object objectValue;
            if (_properties.TryGetValue(tagName, out objectValue))
            {
                value = (T)objectValue;
                return true;
            }
            return false;
        }

        protected Dictionary<string, object> _properties;
    }
}
