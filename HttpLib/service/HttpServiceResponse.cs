using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpLib
{
    /// <summary>
    /// The response from HttpHost after serving a request
    /// </summary>
    public class HttpServiceResponse
    {
        /// <summary>
        /// whether the response is success
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// the result content sent to client
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// the Request from client
        /// </summary>
        public HttpListenerRequest Request { get; set; }

        /// <summary>
        /// the Response to the client
        /// </summary>
        public HttpListenerResponse Response { get; set; }

        /// <summary>
        /// the name for the request handler
        /// </summary>
        public string HandlerName { get; set; }

        /// <summary>
        /// the error message
        /// </summary>
        public string ErrorMessage { get; set; }
    }
}
