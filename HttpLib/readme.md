# HttpLib
This library provides simple utilities for client to send http request and for server to handle http request.

## Client
* HttpRequest - built on top of .NET HttpWebRequest without throwing exceptions when status code is not 2xx
* HttpResponse - the response object that contains result, status, and HttpWebResponse
* HttpClient - simple client to get/post http request

## Service Host
A simple service host built on top of .NET HttpListener. The intent is to provide web service capability on Android phone since it seems ServiceStack no longer supports.
* HttpServiceHost - this is the class that hosts HttpListener and calls GetContextAsync. Upon receiving request, based on the url path, it finds a request handler to process the request.
* HttpServiceRequestHandler - the base class for all request handlers. A static member holds a map of request handlers based on the handler's path it handles.
* HttpServiceEndpoint - defines the endpoint data respond from server
* EndpointRequestHandler - inherit from HttpServiceRequestHandler, EndpointRequestHandler responds with available endpoints that are handled by all request handlers.
* HttpServiceContext and HttpServiceResponse - used by request handler and host to communicate the request context and response.

## Custom Request Handler
All custom request handlers must inherit from the base class HttpServiceRequestHandler. Each handler will be responsible to handle requests to a particular path, for example /cmd/speak.
Note that the service host will select the handler that matches the best. For example, two handlers with path "/A" and "/A/B", the "/A/B" handler will be selected for url "/A/B/C", the "/A" will be selected for url "/A".
The properties for a constructing a request handler:
* Path - this is the url path that will be handled by the handler.
* Name - name of the handler
* Type - this should be the data type that the handler takes/responds
* Credentials - a comma separated list of user:password that are allowed to access the handler's path. No authorization check if it is empty or null.
Please refer to the Riot.Phone for examples of custom request handlers and how to use the service host. The RiotDevices is an Android app that runs the web service.

