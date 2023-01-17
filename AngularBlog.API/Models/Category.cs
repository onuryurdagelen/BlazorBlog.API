using BlazorBlog.API.Abstracts;
using System;
using System.Collections.Generic;

namespace BlazorBlog.API.Models
{
    public partial class Category:IModel
    {
        public Category()
        {
            Articles = new HashSet<Article>();
        }

        public int Id { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<Article> Articles { get; set; }
    }
}
