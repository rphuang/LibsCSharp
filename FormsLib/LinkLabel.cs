using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace FormsLib
{
    /// <summary>
    /// LinkLabel is a Label with clickable link to open a web link
    /// </summary>
    public class LinkLabel : Label
    {
        /// <summary>
        /// construct a LinkLabel with an uri string as label text
        /// </summary>
        public LinkLabel(string labelText)
            : this(new Uri(labelText), labelText)
        {
        }

        /// <summary>
        /// construct a LinkLabel with an uri string and label text
        /// </summary>
        public LinkLabel(string uri, string labelText)
            : this(new Uri(uri), labelText)
        {
        }

        /// <summary>
        /// construct a LinkLabel with an uri and label text
        /// </summary>
        public LinkLabel(Uri uri, string labelText = null)
        {
            Text = labelText ?? uri.ToString();
            TextColor = Color.Blue;
            GestureRecognizers.Add(new TapGestureRecognizer { Command = new Command(() => Device.OpenUri(uri)) });
        }
    }
}
