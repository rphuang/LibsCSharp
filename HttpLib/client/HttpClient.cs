using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net;

namespace HttpLib
{
    /// <summary>
    /// client using http protocol to communicate with server
    /// </summary>
    public class HttpClient
    {
        /// <summary>
        /// constructor
        /// </summary>
        public HttpClient()
        {
        }

        /// <summary>
        /// constructor with server:port and user:password
        /// </summary>
        public HttpClient(string serverAndPort, string userAndPassword)
        {
            SetCredential(userAndPassword);
            SetServer(serverAndPort);
        }

        /// <summary>
        /// set credential with format in user:password
        /// </summary>
        public void SetCredential(string userAndPassword)
        {
            if (string.IsNullOrEmpty(userAndPassword)) return;
            string[] parts = userAndPassword.Split(ColonDelimiter);
            string user = parts[0];
            string password;
            if (parts.Length > 1) password = parts[1];
            else password = string.Empty;
            SetCredential(user, password);
        }

        /// <summary>
        /// set user and password
        /// </summary>
        public void SetCredential(string username, string password)
        {
            _encodedCredential = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(username + ":" + password));
            if (!string.IsNullOrEmpty(_encodedCredential))
            {
                _requestHeaderParams = new Dictionary<string, string>();
                _requestHeaderParams.Add("Authorization", "Basic " + _encodedCredential);
            }
        }

        /// <summary>
        /// set to the specified server:port
        /// </summary>
        public void SetServer(string serverAndPort)
        {
            if (string.IsNullOrEmpty(serverAndPort)) return;
            string[] parts = serverAndPort.Split(ColonDelimiter);
            string server = parts[0];
            int port;
            if (parts.Length > 1) port = int.Parse(parts[1]);
            else port = 80;
            SetServer(server, port);
        }

        /// <summary>
        /// set to the specified server and port
        /// </summary>
        public void SetServer(string server, int port)
        {
            _server = server;
            _port = port;
        }

        /// <summary>
        /// send a get request to server using http protocol
        /// </summary>
        /// <param name="path">the device/component path for the url.</param>
        /// <returns>returns the response from server in HttpResponse</returns>
        public HttpResponse GetResponse(string path)
        {
            string url = string.Format(UrlFormat, _server, _port, path);

            HttpResponse response = _request.Get(url, _requestHeaderParams, true);
            return response;
        }

        /// <summary>
        /// post a command to server using http protocol
        /// </summary>
        /// <param name="path">the device/component path for the url.</param>
        /// <param name="json">the json body to be sent to server.</param>
        /// <returns>returns the response from server</returns>
        public HttpResponse Post(string path, string json)
        {
            string url = string.Format(UrlFormat, _server, _port, path);
            HttpResponse response = _request.Post(url, json, _requestHeaderParams, true);
            _lastMessage = response?.Result;
            _lastStatusCode = response?.Response == null ? 0 : (int)response.Response.StatusCode;
            return response;
        }

        /// <summary>
        /// gets available endpoints from the server
        /// </summary>
        /// <returns>returns a list of endpoints</returns>
        public IList<HttpServiceEndpoint> DiscoverAvailableEndpoints(string path = string.Empty)
        {
            HttpResponse response = GetResponse(path);
            if (response.Success)
            {
                // deserialize
                List<HttpServiceEndpoint> endpoints = JsonConvert.DeserializeObject<List<HttpServiceEndpoint>>(response.Result);
                return endpoints;
            }
            return null;
        }

        /// <summary>
        /// get/set the timeout for request in millisecond
        /// </summary>
        public int RequestTimeout
        {
            get { return _request.RequestTimeout; } 
            set { _request.RequestTimeout = value; }
        }

        private static readonly char[] ColonDelimiter = { ':' };
        private const string UrlFormat = "http://{0}:{1}/{2}";
        private string _server;
        private int _port;
        private string _encodedCredential;
        private IDictionary<string, string> _requestHeaderParams;
        private HttpRequest _request = new HttpRequest();
        private string _lastMessage;
        private int _lastStatusCode;
    }
}
