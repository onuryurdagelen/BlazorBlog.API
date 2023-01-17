using System.ComponentModel.DataAnnotations.Schema;

namespace BlazorBlog.API.Models
{
    public class UserAppClaim
    {
        public int Id { get; set; }
        public int AppClaimId { get; set; }
        [ForeignKey("AppClaimId")]
        public AppClaim AppClaim { get; set; }
        public int UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}
