using BlazorBlog.API.Abstracts;

namespace BlazorBlog.API.Dtos
{
    public class ArchivedArticleDto:IDto
    {
        public int year { get; set; }
        public int month { get; set; }
        public int count { get; set; }
        public string monthName { get; set; }
    }
}
