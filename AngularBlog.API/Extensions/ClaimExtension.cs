using System.Security.Claims;

namespace BlazorBlog.API.Extensions
{
    public static class ClaimExtension
    {
        public static void AddName(this ICollection<Claim> claims, string name)
        {
            claims.Add(new Claim(ClaimTypes.Name, name));
        }
        public static void AddNameIdentifier(this ICollection<Claim> claims, string identifier)
        {
            claims.Add(new Claim(ClaimTypes.NameIdentifier, identifier));
        }
        public static void AddRoles(this ICollection<Claim> claims, string[] roles)
        {
            roles.ToList().ForEach(role => claims.Add(new Claim(ClaimTypes.Role, role)));
        }
        public static void AddEmailAddress(this ICollection<Claim> claims, string emailAddress)
        {
            claims.Add(new Claim(ClaimTypes.Email, emailAddress));
        }
    }
}
