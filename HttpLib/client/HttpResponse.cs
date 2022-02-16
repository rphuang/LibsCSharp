using System.Net;

namespace HttpLib
{
    /// <summary>
    /// encapsulate the response from http web request
    /// </summary>
    public class HttpResponse
    {
        /// <summary>
        /// whether the request is successful
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// the response result from ResponseStream
        /// </summary>
        public string Result { get; set; }

        /// <summary>
        /// error/exception message if any
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// the HttpWebResponse
        /// </summary>
        public HttpWebResponse Response { get; set; }
    }
}
