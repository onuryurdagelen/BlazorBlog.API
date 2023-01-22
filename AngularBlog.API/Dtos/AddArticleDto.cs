using Microsoft.AspNetCore.Components.Forms;

namespace BlazorBlog.API.Dtos
{
    public class AddArticleDto
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public UploadedFileDto UploadedFile { get; set; }
        public int CategoryId { get; set; }

    }
}
