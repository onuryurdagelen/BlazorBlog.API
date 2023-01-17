using BlazorBlog.API.Jwt;
using BlazorBlog.API.Models;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
using System.Security.Cryptography;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using BlazorBlog.API.Extensions;

namespace BlazorBlog.API.Helpers
{
    public class TokenHelper
    {
        private IConfiguration _config { get; set; }
        private readonly TokenOptions _tokenOptions;

        public TokenHelper(IConfiguration configuration)
        {
            _config = configuration;
            _tokenOptions = _config.GetSection("Token").Get<TokenOptions>();
        }
        public Token CreateToken(User user, List<AppClaim> appClaims)
        {
            Token token = new Token();
        
            //Security Key'in simetriği alınır.
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.SecurityKey));

            //Şifrelenmiş kimliği oluşturulur.
            //SigninCredentials class'ı securityKey ve oluşturacağımız security algoritması parametreleri alır.
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //Token ayarları yapılır.
            token.ExpirationTime = DateTime.Now.AddMinutes(60); //Token'ın bitiş süresini 1 dk olarak ayarladık.

            JwtSecurityToken securityToken = new JwtSecurityToken(
                issuer: _tokenOptions.Issuer,
                audience: _tokenOptions.Audience,
                expires: DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration),
                claims: SetClaims(user, appClaims),
                notBefore: DateTime.Now, //"Token üretildikten hemen sonra devreye girsin" demektir.
                signingCredentials: signingCredentials
                );
            //Token oluşturucu sınıfından bir örnek alınır.
            JwtSecurityTokenHandler jwtSecurityTokenHandler = new JwtSecurityTokenHandler();

            //Token üretilir.
            token.AccessToken = jwtSecurityTokenHandler.WriteToken(securityToken);

            //Refresh token üretilir.
            token.RefreshToken = CreateRefreshToken();

            return token;
        }
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
        private IEnumerable<Claim> SetClaims(User user, List<AppClaim> appClaims)
        {
            var claims = new List<Claim>();
            claims.AddName(user.FullName);
            claims.AddEmailAddress(user.EmailAddress);
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddRoles(appClaims.Select(oc => oc.ClaimName).ToArray());

            return claims;

        }
    }
}
