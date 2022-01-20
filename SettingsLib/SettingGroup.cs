using System;
using System.Collections.Generic;
using System.Text;

namespace SettingsLib
{
    /// <summary>
    /// defines a setting group - grouping a collection of settings
    /// in the settings file, setting Group starts with Group=groupId and ends with EndGroup or another group
    /// </summary>
    public class SettingGroup
    {
        /// <summary>
        /// the ID for the setting group
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// the settings inside the group
        /// </summary>
        public IDictionary<string, string> Settings { get; set; } = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// set a setting
        /// </summary>
        public void SetSetting(string name, object value)
        {
            string strValue = value?.ToString();
            if (Settings.ContainsKey(name)) Settings[name] = strValue;
            else Settings.Add(name, strValue);
        }
    }
}
