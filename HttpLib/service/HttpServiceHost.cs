using LogLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace HttpLib
{
    /// <summary>
    /// simple http web service host
    /// </summary>
    public class HttpServiceHost
    {
        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="serverPrefix">the server prefix string for all paths to listen. ex: http://*:5678 </param>
        public HttpServiceHost(string serverPrefix)
        {
            ServerPrefix = serverPrefix;
        }

        /// <summary>
        /// KeepAlive response to client
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// server prefix to listen ex: http://*:5678
        /// </summary>
        public string ServerPrefix { get; set; }

        public IList<string> Prefixes { get; private set; } = new List<string>();

        /// <summary>
        /// initialize 
        /// </summary>
        public virtual HttpServiceHost Init()
        {
            _listener = new HttpListener();
            foreach (var item in HttpServiceRequestHandler.Handlers)
            {
                string prefix = ServerPrefix + item.Key;
                Prefixes.Add(prefix);
                if (!prefix.EndsWith("/")) prefix += "/";
                _listener.Prefixes.Add(prefix);
            }
            string csv = string.Join(",", Prefixes);
            Log.Action($"HttpServiceHost initialized root: {ServerPrefix} handlers: {Prefixes.Count} listening: {csv}");
            return this;
        }

        /// <summary>
        /// start listener
        /// </summary>
        public virtual void Start()
        {
            Log.Action($"HttpServiceHost starting");
            _listener.Start();
        }

        /// <summary>
        /// stop listener
        /// </summary>
        public virtual void Stop()
        {
            Log.Action($"HttpServiceHost stopping");
            _listener.Stop();
        }

        /// <summary>
        /// get context
        /// </summary>
        /// <returns></returns>
        public async Task<HttpServiceResponse> GetContextAsync()
        {
            HttpListenerContext context = await _listener.GetContextAsync();
            HttpListenerRequest request = context.Request;
            HttpListenerResponse response = context.Response;
            Log.Action($"{request.HttpMethod} {request.Url.AbsoluteUri}");

            HttpServiceRequestHandler handler;
            string matchedPath;
            List<string> unmatchedSegments;
            if (!GetRequestHandler(request.Url.AbsolutePath, out handler, out matchedPath, out unmatchedSegments))
            {
                return HttpServiceRequestHandler.CreateResponseForBadRequest(new HttpServiceContext { Context = context, MatchedPath = matchedPath }, $"NotSupported: {request.Url.AbsolutePath}");
            }

            HttpServiceContext hostContext = new HttpServiceContext { Context = context, MatchedPath = matchedPath, UnmatchedSegments = unmatchedSegments };
            HttpServiceResponse hostResponse = null;
            try
            {
                hostResponse = handler.ProcessRequest(hostContext);
            }
            catch (Exception err)
            {
                hostResponse = HttpServiceRequestHandler.CreateResponseForInternalError(hostContext, handler.Name, err);
            }

            try
            {
                if (hostResponse != null)
                {
                    if (hostResponse.Content == null) hostResponse.Content = string.Empty;
                    // send content to response
                    byte[] buffer = Encoding.UTF8.GetBytes(hostResponse.Content);
                    // Get a response stream and write the response
                    response.ContentEncoding = Encoding.UTF8;
                    response.ContentLength64 = buffer.Length;
                    response.KeepAlive = KeepAlive;
                    Stream output = response.OutputStream;
                    output.Write(buffer, 0, buffer.Length);
                    // must close the output stream.
                    output.Close();

                    if (string.IsNullOrEmpty(response.ContentType)) response.ContentType = "application/json";
                }
                else
                {
                    hostResponse = HttpServiceRequestHandler.CreateResponseForInternalError(hostContext, handler.Name);
                }
            }
            catch (Exception err)
            {
                hostResponse = HttpServiceRequestHandler.CreateResponseForInternalError(hostContext, handler.Name, err);
            }
            Log.Action($"{request.HttpMethod} {request.Url.AbsoluteUri} Status: {response.StatusCode} Result: {hostResponse.Content} Error: {hostResponse.ErrorMessage}");
            return hostResponse;
        }

        private bool GetRequestHandler(string absolutePath, out HttpServiceRequestHandler handler, out string matchedPath, out List<string> unmatchedSegments)
        {
            handler = null;
            matchedPath = null;
            unmatchedSegments = null;
            string path = absolutePath;
            // first: find handler that matches the whole path
            if (HttpServiceRequestHandler.Handlers.TryGetValue(absolutePath, out handler))
            {
                matchedPath = absolutePath;
            }
            else
            {
                // then remove the last segment of the path and find a match until reaching "/"
                while (true)
                {
                    int index = path.LastIndexOf('/');
                    if (index == 0)
                    {
                        if (HttpServiceRequestHandler.Handlers.TryGetValue("/", out handler)) matchedPath = "/";
                        break;
                    }
                    else
                    {
                        string unmatched = path.Substring(index + 1);
                        if (!string.IsNullOrEmpty(unmatched))
                        {
                            if (unmatchedSegments == null) unmatchedSegments = new List<string>();
                            unmatchedSegments.Insert(0, unmatched);
                        }
                        path = path.Substring(0, index);
                        if (HttpServiceRequestHandler.Handlers.TryGetValue(path, out handler))
                        {
                            matchedPath = path;
                            break;
                        }
                    }
                }
            }
            return handler != null && !string.IsNullOrEmpty(matchedPath);
        }

        private HttpListener _listener;
    }
}
