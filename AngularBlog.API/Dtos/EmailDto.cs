using BlazorBlog.API.Abstracts;

namespace BlazorBlog.API.Dtos
{
    public class ContactEmailDto:IDto
    {
        public string FullName { get; set; } = string.Empty;
        public string EmailAddress { get; set; } = string.Empty;
        public string SubjectTitle { get; set; } = string.Empty;
        public string SubjectContent { get; set; } = string.Empty;
    }
}
