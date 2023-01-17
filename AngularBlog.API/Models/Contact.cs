using BlazorBlog.API.Abstracts;

namespace BlazorBlog.API.Models
{
    public class Contact:IModel
    {
        public string Name { get; set; }
        //public string EmailAdress { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

    }
}
