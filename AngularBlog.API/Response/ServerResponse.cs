using BlazorBlog.API.Abstracts;
using System.Net;

namespace BlazorBlog.API.Response
{
    public class ServerResponse : IServerResponse
    {
        public string Error { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
    }
}
