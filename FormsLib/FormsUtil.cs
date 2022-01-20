using System;
using System.Collections.Generic;
using System.IO;
using Xamarin.Forms;
using static Xamarin.Forms.Grid;

namespace FormsLib
{
    /// <summary>
    /// Provides helper utilities for for Xamarin Forms
    /// </summary>
    public static class FormsUtil
    {
        // constant for default max string length displayed
        public const int MaxValueStringLengthToDisplay = 47;

        // constant for the size of font label
        public static double LabelDefaultFontSize = Device.GetNamedSize(NamedSize.Default, typeof(Label));
        public static double LabelMediumFontSize = Device.GetNamedSize(NamedSize.Medium, typeof(Label));

        /// <summary>
        /// create a label or LinkLabel when Uri is not null
        /// <returns></returns>
        public static Label CreateLabel(string value, Uri uri, int maxLength = MaxValueStringLengthToDisplay)
        {
            if (string.IsNullOrEmpty(value)) return null;

            string tmp = value;
            // clamp the value string to avoid double row spacing
            if (maxLength > 0 && value.Length > maxLength)
            {
                tmp = value.Substring(0, MaxValueStringLengthToDisplay - 3) + "...";
            }
            Label label;
            // use LinkLabel when the value starts with http:// or https://
            if (uri != null) label = new LinkLabel(uri, tmp);
            else label = new Label { Text = tmp };

            label.MaxLines = 1;
            return label;
        }
    }
}
