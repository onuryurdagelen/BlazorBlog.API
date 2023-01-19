using System;
using System.Collections.Generic;

namespace BlazorBlog.API.Models
{
    public partial class User
    {
        public User()
        {
            UserAppClaims = new HashSet<UserAppClaim>();
        }

        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string EmailAddress { get; set; } = null!;
        public byte[] PasswordHash { get; set; } = null!;
        public byte[] PasswordSalt { get; set; } = null!;
        public DateTime CreatedDate { get; set; }
        public DateTime? UpdatedDate { get; set; }

        public virtual ICollection<UserAppClaim> UserAppClaims { get; set; }
    }
}
