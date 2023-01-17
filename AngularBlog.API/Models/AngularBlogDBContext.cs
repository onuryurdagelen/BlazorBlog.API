using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorBlog.API.Models
{
    public partial class AngularBlogDBContext : DbContext
    {
        public AngularBlogDBContext()
        {
        }

        public AngularBlogDBContext(DbContextOptions<AngularBlogDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<AppClaim> AppClaims { get; set; } = null!;
        public virtual DbSet<UserAppClaim> UserAppClaims { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-OKIVNDG;Initial Catalog=AngularBlogDB;User ID=onur;Password=12345onur;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=False;");
            }
        }

       

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
