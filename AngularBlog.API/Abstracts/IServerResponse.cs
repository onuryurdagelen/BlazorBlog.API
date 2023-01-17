using System.Net;

namespace BlazorBlog.API.Abstracts
{
    public interface IServerResponse
    {
        string Error { get; set; }
        bool IsSuccess { get; set; }
        HttpStatusCode StatusCode { get; set; }
    }
}
