using BlazorBlog.API.Dtos;
using BlazorBlog.API.Helpers;
using BlazorBlog.API.Jwt;
using BlazorBlog.API.Models;
using BlazorBlog.API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;

namespace BlazorBlog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        private readonly BlazorBlogDBContext _context;
        private readonly IConfiguration _config;

        public AuthController(BlazorBlogDBContext context, IConfiguration configuration)
        {
            _context = context;
            _config = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<ServerDataResponse<UserRegisterDto>>> Register(UserRegisterDto userRegisterDto)
        {
            ServerDataResponse<UserRegisterDto> response = new ServerDataResponse<UserRegisterDto>();
            /* password hash ve password salt oluştur.
             * yeni bir user nesnesi oluştur.Password salt ve password hash'i user nesnesine al.
             * user nesnesi oluşturulduktan sonra veritabanında bu kullanıcının'ın varlığını kontrol et.
             * Eğer bu kullanıcı veritabanında var ise kullanıcıya kullanıcının var olduğunu gösteren hata kodu ve hata mesajını gönder.
             * Eğer bu kullanıcı veritabanında yok ise bu kullanıcıyı veritabanına kaydet.Başarılı kod ve mesaj bilgisi gönder.
             * Kullanıcı ile beraber kullanıcının claim bilgilerini jwt token ile kullanıcıya gönder.
             * 
             */
            byte[] passwordHash, passwordSalt;
            HashingHelper.CreatePasswordHash(userRegisterDto.Password, out passwordHash, out passwordSalt);
            User user = new User()
            {
                CreatedDate = DateTime.UtcNow,
                EmailAddress = userRegisterDto.EmailAddress,
                FullName = userRegisterDto.FullName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
            };
            User checkedUser = await _context.Users.Where(x => x.EmailAddress == userRegisterDto.EmailAddress).FirstOrDefaultAsync();

            if (checkedUser != null) {
                response.Error = "Such a user already exists.";
                response.IsSuccess = false;
                response.Data = null;
                response.StatusCode = System.Net.HttpStatusCode.BadRequest;

                return BadRequest(response);
            }
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            response.IsSuccess = true;
            response.Message = "Registration successfully done.";
            response.StatusCode = System.Net.HttpStatusCode.OK;
            response.Data = userRegisterDto;

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<ServerDataResponse<Token>>> Login(UserLoginDto userLoginDto)
        {
            ServerDataResponse<Token> response = new ServerDataResponse<Token>();

            var existedUser = await _context.Users.Where(x => x.EmailAddress == userLoginDto.EmailAddress).FirstOrDefaultAsync();
            
            //Böyle bir kullanıcı var ise
            if (existedUser != null)
            {
                var verifiedUser = HashingHelper.VerifyPasswordHash(userLoginDto.Password, existedUser.PasswordHash, existedUser.PasswordSalt);
                //Var olan kullanıcı ile login bilgileri eşleşiyor ise
                if (verifiedUser)
                {
                    //kullanıcıya ait claim'leri al.

                    List<UserAppClaim> userAppClaims = await _context.UserAppClaims.Where(x => x.UserId == existedUser.Id).ToListAsync();

                    List<AppClaim> appClaims = new List<AppClaim>();

                    foreach (var item in userAppClaims)
                    {
                        AppClaim appClaim = await _context.AppClaims.Where(x => x.Id == item.AppClaimId).FirstOrDefaultAsync();
                        appClaims.Add(appClaim);
                    }

                    //Token oluştur.
                    TokenHelper tokenHelper = new TokenHelper(_config);

                    Token token = tokenHelper.CreateToken(existedUser, appClaims);
                    response.Data = token;
                    response.IsSuccess = true;
                    response.Message = "login successful";
                    response.StatusCode = System.Net.HttpStatusCode.OK;

                    return Ok(response);
                }
            }
            response.Message = "No such a user exists.";
            response.IsSuccess = false;
            response.StatusCode = System.Net.HttpStatusCode.NotFound;
            response.Data = null;
            return NotFound(response);
        }
    }
}
