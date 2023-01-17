using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.API.Dtos
{
    public class UserRegisterDto
    {
        public string EmailAddress { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
