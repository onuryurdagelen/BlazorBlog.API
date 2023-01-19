using BlazorBlog.API.Abstracts;
using System.ComponentModel.DataAnnotations;

namespace BlazorBlog.API.Dtos
{
    public class UserRegisterDto:IDto
    {
        public string EmailAddress { get; set; }
        public string FullName { get; set; }
        public string Password { get; set; }
        public string PasswordConfirm { get; set; }
    }
}
