using BlazorBlog.API.Abstracts;
using System.Net;
using System.Text.Json;

namespace BlazorBlog.API.Response
{
    public class ServerDataResponse<TResponse> : IServerDataResponse<TResponse> where TResponse : class,new()
    {
        #region Variables
        public TResponse Data { get; set; }
        public string Error { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        #endregion

        #region Constructors
        public ServerDataResponse()
        {
            Type type = typeof(TResponse);
            Data = Activator.CreateInstance(type) as TResponse; //Requires parameterless constructor.

            this.Error = null;
            this.Message = null;
            this.IsSuccess = false;
        } // End of the constructor
        #endregion

        #region Get methods
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        } // End of the ToString method
        #endregion
    } // End of the class
}
