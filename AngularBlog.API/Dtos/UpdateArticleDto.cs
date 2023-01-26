namespace BlazorBlog.API.Dtos
{
    public class UpdateArticleDto
    {
        public int ArticleId { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Picture { get; set; }
        public string ImagePath { get; set; }
        public UploadedFileDto? UploadedFile { get; set; }
        public int CategoryId { get; set; }

    }
}
