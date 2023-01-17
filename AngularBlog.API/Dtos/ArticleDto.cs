using BlazorBlog.API.Abstracts;
using BlazorBlog.API.Models;

namespace BlazorBlog.API.Dtos
{
    public class ArticleDto:IDto
    {
        public int TotalCount { get; set; }
        public List<Article> Articles { get; set; }
    }
}
