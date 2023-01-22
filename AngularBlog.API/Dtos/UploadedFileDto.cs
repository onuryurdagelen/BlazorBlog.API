namespace BlazorBlog.API.Dtos
{
    public class UploadedFileDto
    {
        public string FileName { get; set; }
        public long FileSize { get; set; }
        public string FileExtension { get; set; }
        public byte[] FileContent { get; set; }
    }
}
