using BlazorBlog.API.Abstracts;

namespace BlazorBlog.API.Jwt
{
    public class Token :IDto
    {
        public string AccessToken { get; set; }
        public DateTime ExpirationTime { get; set; }
        public string RefreshToken { get; set; }
    }
}
