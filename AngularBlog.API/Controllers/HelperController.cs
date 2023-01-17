using BlazorBlog.API.Dtos;
using BlazorBlog.API.Models;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Net;
using System.Net.Mail;
//using MimeKit; //Mail göndermek için kullanılan kütüphanedir.
namespace BlazorBlog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class HelperController : ControllerBase
    {
        private readonly IConfiguration _config;

        public HelperController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public IActionResult SendContactEmail(ContactEmailDto emailDto)
        {
            try
            {

                MailMessage mailMessage = new MailMessage();

                System.Net.Mail.SmtpClient smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                mailMessage.From = new MailAddress(emailDto.EmailAddress);
                mailMessage.To.Add("yurdagelenonur1@gmail.com");

                mailMessage.Subject = emailDto.SubjectTitle;
                mailMessage.Body = emailDto.SubjectContent;
                mailMessage.IsBodyHtml = true;
                smtpClient.Port = 587;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential("yurdagelenonur1@gmail.com", "becnzjsgiwkumglx");

                smtpClient.Send(mailMessage);



                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient()
                //{
                //    Host = "smtp.gmail.com",
                //    Port = 587,
                //    EnableSsl = true,
                //    DeliveryMethod = SmtpDeliveryMethod.Network,
                //    UseDefaultCredentials = false,
                //    Credentials = new NetworkCredential()
                //    {
                //        UserName = "yurdagelenonur1@gmail.com",
                //        Password = "becnzjsgiwkumglx"
                //    }
                //};

                //MailAddress FromEmail = new MailAddress(emailDto.From, "Gym");
                //MailAddress ToEmail = new MailAddress("yurdagelenonur1@gmail.com", "Someone");
                
                //MailMessage Message = new MailMessage()
                //{
                //    From = FromEmail,
                //    Subject = emailDto.Subject,
                //    Body = emailDto.Body,
                //};
                //Message.To.Add(ToEmail);
                //smtp.Send(Message);

            
                return Ok();
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);
            }
        }
    }
}
