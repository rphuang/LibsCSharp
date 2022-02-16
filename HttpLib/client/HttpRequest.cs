using LogLib;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Text;

namespace HttpLib
{
    public class HttpRequest
    {
        /// <summary>
        /// whether to parse the response json and return the result of value property
        /// </summary>
        public bool ParseJsonToReturnFirstValue { get; set; } = false;

        /// <summary>
        /// the timeout for request in millisecond
        /// </summary>
        public int RequestTimeout { get; set; } = 10000;

        /// <summary>
        /// Get result from specified url
        /// </summary>
        /// <param name="id">The url to request</param>
        /// <returns>the json string response</returns>
        public string Get(string url, bool keepAlive = false)
        {
            HttpResponse response = Get(url, null, keepAlive);
            return response.Result;
        }

        /// <summary>
        /// Get response from specified url
        /// </summary>
        /// <returns>the json string response</returns>
        public string Get(string url, IDictionary<string, string> requestHeaderParams, out HttpWebResponse response, bool keepAlive)
        {
            HttpResponse httpResponse = Get(url, requestHeaderParams, keepAlive);
            response = httpResponse?.Response;
            return httpResponse?.Result;
        }

        /// <summary>
        /// Get response from specified url
        /// </summary>
        public HttpResponse Get(string url, IDictionary<string, string> requestHeaderParams, bool keepAlive)
        {
            bool success = false;
            string errorMessage = null;
            HttpWebResponse response = null;
            string json = null;
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "GET";
            httpWebRequest.KeepAlive = keepAlive;
            httpWebRequest.Timeout = RequestTimeout;
            Log.Info("Requesting Url: {0}", url);
            if (requestHeaderParams != null)
            {
                foreach (var item in requestHeaderParams)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }
            OnPreRequest(httpWebRequest, url);

            try
            {
                Stopwatch timer = new Stopwatch();
                timer.Start();
                response = (HttpWebResponse)httpWebRequest.GetResponse();
                long elapsedMilliseconds = timer.ElapsedMilliseconds;
                response.Headers.Add("RequestResponseElapsedMilliseconds", elapsedMilliseconds.ToString());

                if ((int)response.StatusCode >= 300)
                {
                    _httpErrorCount++;
                    Log.Error("Request Error {0} for {1}", response.StatusCode, url);
                }
                // read the stream associated with the response.
                json = GetResponseStream(response);
                elapsedMilliseconds = timer.ElapsedMilliseconds;
                response.Headers.Add("RequestCompletedElapsedMilliseconds", elapsedMilliseconds.ToString());
                timer.Stop();

                OnAfterRequest(response, json);
                Log.Info("Response status:{0} Content length:{1} type:{2} Url:{3}",
                    response.StatusCode, response.ContentLength, response.ContentType, url);
                success = true;
            }
            catch (WebException ex)
            {
                _exceptionCount++;
                response = (HttpWebResponse)ex.Response;
                errorMessage = ex.ToString();
                //Log.Error("Exception on getting {0}: {1}", url, ex.ToString());
                if (response != null)
                {
                    try
                    {
                        json = GetResponseStream(response);
                    }
                    catch { }
                }
            }
            catch (System.Exception ex)
            {
                _exceptionCount++;
                errorMessage = ex.ToString();
                Log.Error("Exception on getting {0}: {1}", url, ex.ToString());
            }
            return new HttpResponse { Success = success, Result = json, Response = response, ErrorMessage = errorMessage };
        }

        /// <summary>
        /// post request to specified url
        /// </summary>
        public HttpResponse Post(string url, string json, IDictionary<string, string> requestHeaderParams, bool keepAlive)
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";
            httpWebRequest.KeepAlive = keepAlive;
            if (requestHeaderParams != null)
            {
                foreach (var item in requestHeaderParams)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }

            HttpWebResponse httpResponse = null;
            bool success = true;
            string errorMessage = null;
            string result = null;
            try
            {
                OnPrePost(httpWebRequest, url, json);

                var data = Encoding.ASCII.GetBytes(json);
                httpWebRequest.ContentLength = data.Length;
                using (var stream = httpWebRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                Stopwatch timer = new Stopwatch();
                timer.Start();
                httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    result = streamReader.ReadToEnd();
                }
                OnAfterPost(httpResponse, result);
                success = (int)httpResponse.StatusCode < 300;
            }
            catch (System.Exception ex)
            {
                success = false;
                errorMessage = $"Exception on posting to {url}: {ex.ToString()}";
                Log.Error(errorMessage);
            }
            return new HttpResponse { Success = success, Result = result, Response = httpResponse, ErrorMessage = errorMessage };
        }

        /// <summary>
        /// Virtual method allows derived class to manipulate request headers for Get
        /// </summary>
        /// <param name="request">HTTP web request.</param>
        /// <param name="url">The url used for creating request.</param>
        protected virtual void OnPreRequest(HttpWebRequest request, string url)
        {
        }

        /// <summary>
        /// Virtual method allows derived class to monitor request 
        /// </summary>
        /// <param name="response">response for the HTTP web request.</param>
        /// <param name="text">The string returned.</param>
        protected virtual void OnAfterRequest(HttpWebResponse response, string text)
        {
        }

        /// <summary>
        /// Virtual method allows derived class to manipulate request headers for Post
        /// </summary>
        /// <param name="request">HTTP web request.</param>
        /// <param name="url">The url used for creating request.</param>
        /// <param name="json">The body for the post request.</param>
        protected virtual void OnPrePost(HttpWebRequest request, string url, string json)
        {
        }

        /// <summary>
        /// Virtual method allows derived class to monitor request 
        /// </summary>
        /// <param name="response">response for the HTTP web request.</param>
        /// <param name="text">The string returned.</param>
        protected virtual void OnAfterPost(HttpWebResponse response, string text)
        {
        }

        protected string GetResponseStream(HttpWebResponse response)
        {
            string result = null;
            // read the stream associated with the response.
            using (StreamReader readStream = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = (readStream.ReadToEnd());
                if (ParseJsonToReturnFirstValue)
                {
                    const string valueString = "\"value\": ";
                    int valueIndex = result.IndexOf(valueString);
                    if (valueIndex > 0)
                    {
                        valueIndex += valueString.Length;
                        result = result.Substring(valueIndex, result.Length - valueIndex - 1);
                    }
                }
            }
            return result;
        }

        private int _exceptionCount = 0;
        private int _httpErrorCount = 0;
    }
}
