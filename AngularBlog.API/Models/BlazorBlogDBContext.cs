using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BlazorBlog.API.Models
{
    public partial class BlazorBlogDBContext : DbContext
    {
        public BlazorBlogDBContext()
        {
        }

        public BlazorBlogDBContext(DbContextOptions<BlazorBlogDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppClaim> AppClaims { get; set; } = null!;
        public virtual DbSet<Article> Articles { get; set; } = null!;
        public virtual DbSet<Category> Categories { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<User> Users { get; set; } = null!;
        public virtual DbSet<UserAppClaim> UserAppClaims { get; set; } = null!;



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AppClaim>(entity =>
            {
                entity.Property(e => e.ClaimName).HasMaxLength(50);
            });

            modelBuilder.Entity<Article>(entity =>
            {
                entity.Property(e => e.ContentSummary).HasMaxLength(200);

                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.HasOne(d => d.Category)
                    .WithMany(p => p.Articles)
                    .HasForeignKey(d => d.CategoryId)
                    .HasConstraintName("FK_Articles_Categories");
            });

            modelBuilder.Entity<Category>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.Property(e => e.Name)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PublishDate).HasColumnType("datetime");

                entity.HasOne(d => d.Article)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.ArticleId)
                    .HasConstraintName("FK_Comments_Articles");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.UpdatedDate).HasColumnType("date");
            });

            modelBuilder.Entity<UserAppClaim>(entity =>
            {
                entity.HasOne(d => d.AppClaim)
                    .WithMany(p => p.UserAppClaims)
                    .HasForeignKey(d => d.AppClaimId)
                    .HasConstraintName("FK_UserAppClaims_AppClaims");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserAppClaims)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK_UserAppClaims_Users");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
