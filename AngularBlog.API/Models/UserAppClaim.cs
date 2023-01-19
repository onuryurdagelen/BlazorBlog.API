using System;
using System.Collections.Generic;

namespace BlazorBlog.API.Models
{
    public partial class UserAppClaim
    {
        public int Id { get; set; }
        public int AppClaimId { get; set; }
        public int UserId { get; set; }

        public virtual AppClaim AppClaim { get; set; } = null!;
        public virtual User User { get; set; } = null!;
    }
}
