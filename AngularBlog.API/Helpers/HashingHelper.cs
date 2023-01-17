using System.Text;

namespace BlazorBlog.API.Helpers
{
    public static class HashingHelper
    {
        public static void CreatePasswordHash(string Password, out byte[] PasswordHash, out byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                PasswordSalt = hmac.Key;
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
            }
        }
        public static bool VerifyPasswordHash(string Password, byte[] PasswordHash, byte[] PasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(PasswordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password));
                for (int i = 0; i < computeHash.Length; i++)
                {
                    if (computeHash[i] != PasswordHash[i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
