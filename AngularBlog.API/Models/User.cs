namespace BlazorBlog.API.Models
{
    public class User
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public byte[] PassswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }
    }
}
