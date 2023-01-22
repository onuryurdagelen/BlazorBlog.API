using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlazorBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileUploadsController : ControllerBase
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public FileUploadsController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        [HttpPost]
        public async Task<IActionResult> Post([FromForm] IFormFile image)
        {
            if (image == null || image.Length == 0) {
                return BadRequest("Upload eny Image");
            }
            string fileName = image.FileName;
            string extension = Path.GetExtension(fileName);

            string[] allowedExtensions = { ".jpg", ".png", ".jpeg" };

            if(!allowedExtensions.Contains(extension)) 
            { 
                return BadRequest("Invalid Image.Try Another");

            }
            string fileNameNew = $"{Guid.NewGuid().ToString()}{extension}";
            string filePath = Path.Combine(_webHostEnvironment.ContentRootPath, "wwwroot","assets/images/articles",fileNameNew);

            using(var fileStream = new FileStream(filePath, FileMode.Create,FileAccess.Write))
            {
                await image.CopyToAsync(fileStream);
            }
            return Ok("https://" + Request.Host + "/assets/images/articles/" + fileNameNew);
        }
    }
}
