using BlazorBlog.API.Abstracts;
using System.Net;

namespace BlazorBlog.API.Response
{
    public class IServerDataResponse<TResponse> where TResponse : class,new()
    {
        TResponse Data { get; }
        string Error { get; }
        bool IsSuccess { get; }
        HttpStatusCode StatusCode { get; }
    }
}
