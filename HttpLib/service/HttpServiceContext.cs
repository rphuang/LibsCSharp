using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace HttpLib
{
    /// <summary>
    /// defines the context for handling request
    /// </summary>
    public class HttpServiceContext
    {
        /// <summary>
        /// the path that matched to the request
        /// </summary>
        public string MatchedPath { get; set; }

        /// <summary>
        /// segments of path that had not been matched to the handler's path
        /// example: client request: "/a/b/c/d" handler path: "/a/b" then unmatched segments: {"c", "d"}
        /// </summary>
        public IList<string> UnmatchedSegments { get; set; }

        /// <summary>
        /// the original HttpListenerContext from HttpListener
        /// </summary>
        public HttpListenerContext Context { get; set; }
    }
}
