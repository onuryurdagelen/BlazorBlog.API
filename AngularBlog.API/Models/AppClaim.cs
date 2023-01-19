using System;
using System.Collections.Generic;

namespace BlazorBlog.API.Models
{
    public partial class AppClaim
    {
        public AppClaim()
        {
            UserAppClaims = new HashSet<UserAppClaim>();
        }

        public int Id { get; set; }
        public string ClaimName { get; set; } = null!;

        public virtual ICollection<UserAppClaim> UserAppClaims { get; set; }
    }
}
