using BlazorBlog.API.BusinessRules;
using BlazorBlog.API.Dtos;
using BlazorBlog.API.Models;
using BlazorBlog.API.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace BlazorBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly BlazorBlogDBContext _context;

        public ArticlesController(BlazorBlogDBContext context)
        {
            _context = context;
        }
        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<ServerDataResponse<List<Article>>>> GetArticles()
        {
            List<Article> articles = await _context.Articles.ToListAsync();

            ServerDataResponse<List<Article>> response = new ServerDataResponse<List<Article>>()
            {
                Data = articles,
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,

            };
            return Ok(response);


        }
        [HttpGet("{page}/{pageSize}")]
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArticlesByPaginately(int page = 1, int pageSize = 5)
        {
            IQueryable<Article> query;
            query = _context.Articles.Include(x => x.Category).Include(x => x.Comments).OrderByDescending(x => x.PublishDate);
            int totalCount = query.Count();

            List<Article> articles = query.Skip(pageSize * (page - 1)).Take(5).ToList();

            ServerDataResponse<ArticleDto> response = new ServerDataResponse<ArticleDto>()
            {
                Data =
                {
                    Articles = articles,
                    TotalCount = totalCount
                },
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,

            };
            return Ok(response);
        }

        // GET: api/Articles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerDataResponse<Article>>> GetArticle(int id)
        {
            ServerDataResponse<Article> response = new ServerDataResponse<Article>();
            var article = await _context.Articles.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (article == null)
            {
                response.Message = "Article Not Found";
                response.IsSuccess = false;
                response.StatusCode = System.Net.HttpStatusCode.NotFound;
                return NotFound(response);
            }
            response.Message = "Başarılı";
            response.Data = article;
            response.IsSuccess = true;
            response.StatusCode = System.Net.HttpStatusCode.OK;
            return Ok(response);
        }

        // PUT: api/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle(int id, Article article)
        {
            if (id != article.Id)
            {
                return BadRequest();
            }

            _context.Entry(article).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Article>> PostArticle(Article article)
        {
            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetArticle", new { id = article.Id }, article);
        }

        // DELETE: api/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
        [HttpGet("ArticlesAsCategory/{categoryId}/{page?}/{pageSize?}")]
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArticlesByCategoryId(int categoryId, int page = 1, int pageSize = 5)
        {

            IQueryable<Article> query;
            query = _context.Articles.Include(x => x.Category).Where(x => x.CategoryId == categoryId).OrderByDescending(x => x.PublishDate);
            int totalCount = query.Count();
            List<Article> articles = await query.Skip(pageSize * (page - 1)).Take(5).ToListAsync();

            ServerDataResponse<ArticleDto> response = new ServerDataResponse<ArticleDto>()
            {
                Data =
                {
                    Articles = articles,
                    TotalCount = totalCount
                },
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,

            };
            return Ok(response);
        }
        [HttpGet("MostViewedArticles")]
        public async Task<ActionResult<ServerDataResponse<List<Article>>>> GetMostViewedArticles()
        {
            ServerDataResponse<List<Article>> response = new ServerDataResponse<List<Article>>();
            IQueryable<Article> query;
            query = _context.Articles.OrderByDescending(x => x.ViewCount).Take(5);
            List<Article> result= await query.ToListAsync();

            if(result.Count > 0)
            {
                response.Data = result;
                response.IsSuccess = true;
                response.StatusCode = System.Net.HttpStatusCode.OK;
                
                return Ok(response); 
            
            }
            response.StatusCode=System.Net.HttpStatusCode.BadRequest;
            response.Error = "Hata oluştu.";
            response.IsSuccess=false;
            return BadRequest(response);

        }
        [HttpGet("ArchivesArticles/{tillTheYears:int}")]
        public async Task<ActionResult<ServerDataResponse<List<ArchivedArticleDto>>>> GetArchivesArticles(int tillTheYears = 5)
        {
            var query = _context.Articles.Where(x => x.PublishDate >= DateTime.Now.AddYears(-(tillTheYears))).GroupBy(x => new
            {
                x.PublishDate.Year,
                x.PublishDate.Month
            }).Select(y =>
            new
            {
                year = y.Key.Year,
                month = y.Key.Month,
                count = y.Count(),
                monthName = CultureInfo.CreateSpecificCulture("en-US").DateTimeFormat.GetMonthName(y.Key.Month),

            }).OrderByDescending(z => z.year);

            List<ArchivedArticleDto> archivedArticlesDto = new List<ArchivedArticleDto>();

            query.ToList().ForEach(x =>
            {
                ArchivedArticleDto arch = new ArchivedArticleDto()
                {
                    count = x.count,
                    month = x.month,
                    monthName = x.monthName,
                    year = x.year,
                };
                archivedArticlesDto.Add(arch);  
            });
            ServerDataResponse<List<ArchivedArticleDto>> responseObj = new ServerDataResponse<List<ArchivedArticleDto>>()
            {
                Data = archivedArticlesDto,
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,

            };

            return Ok(responseObj);
        }
        [HttpGet("ListOfArchivedArticles/{year}/{month}/{page}/{pageSize}")]
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArchivedArticlesByYearAndMonth(int year, int month, int page = 1, int pageSize = 5)
        {
            IQueryable<Article> query;
            query = _context.Articles.Where(x => x.PublishDate.Month == month && x.PublishDate.Year == year)
                .Include(x => x.Category).Include(x => x.Comments).OrderByDescending(x => x.PublishDate);
            int totalCount = query.Count();

            List<Article> articles = query.Skip(pageSize * (page - 1)).Take(5).ToList();

            ServerDataResponse<ArticleDto> response = new ServerDataResponse<ArticleDto>()
            {
                Data =
                {
                    Articles = articles,
                    TotalCount = totalCount
                },
                IsSuccess = true,
                StatusCode = System.Net.HttpStatusCode.OK,

            };
            return Ok(response);
        }


        [HttpPost]
        [Route("AddArticle")]
        public async Task<ActionResult<ServerDataResponse<AddArticleDto>>> InsertArticle(AddArticleDto addArticleDto)
        {
            ServerDataResponse<AddArticleDto> Response = new ServerDataResponse<AddArticleDto>();
            try
            {
                string fileExtension = Path.GetExtension(addArticleDto.UploadedFile.FileName);

                ServerDataResponse<AddArticleDto> checkedFileExtension = ArticleManager.CheckFileExtension(fileExtension);

                if (!checkedFileExtension.IsSuccess)
                {
                    return NotFound(checkedFileExtension);
                }

                ServerDataResponse<AddArticleDto> checkedFileSize = ArticleManager.CheckFileSize(addArticleDto.UploadedFile.FileSize);

                if(!checkedFileSize.IsSuccess) 
                {
                    return NotFound(checkedFileSize);
                }

                //resim adı tanımlanır.
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(addArticleDto.UploadedFile.FileName);

                //kaydedilen yer belirtilir.
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/assets/images/articles", fileName);

                //dosyayı yazabilmek için stream açılır

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    //gönderdiğimiz dosyayı stream'e kopyalarız.
                    stream.Write(addArticleDto.UploadedFile.FileContent,0, addArticleDto.UploadedFile.FileContent.Length);
                };
                Article article = new Article()
                {
                    CategoryId = addArticleDto.CategoryId,
                    Name = addArticleDto.Title,
                    ContentSummary = addArticleDto.Content.Substring(0, addArticleDto.Content.Length / 2),
                    ContentMain = addArticleDto.Content,
                    PublishDate = DateTime.Now,
                    Picture = "https://" + Request.Host + "/assets/images/articles/" + fileName,
                    ViewCount = 0
                };
                _context.Articles.Add(article);
                _context.SaveChanges();

                Response.Data = addArticleDto;
                Response.IsSuccess = true;
                Response.Message = "Succesful.";

                return Ok(Response);
            }
            catch (Exception ex)
            {
                Response.Error = ex.Message;
                Response.IsSuccess = false;
                Response.Message = "Something went wrong!";
                
                return StatusCode(500,Response);
            }
        }

        [HttpPost]
        [Route("SaveArticlePicture")]
        public async Task<IActionResult> SaveArticlePicture(IFormFile picture)
        {
            //resim adı tanımlanır.
            string fileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

            //kaydedilen yer belirtilir.
            string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwwroot/assets/images/articles", fileName);

            //dosyayı yazabilmek için stream açılır

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                //gönderdiğimiz dosyayı stream'e kopyalarız.
                await picture.CopyToAsync(stream);
            };

            //path'in yolunu database'e göndeririz.

            //wwwroot adını vermemize gerek yoktur.static dosyaların yeri her zaman wwwroot'tur.

            var result =new 
            {
                path = "https://"+Request.Host + "/assets/images/articles/"+fileName
             };

            return Ok();
        }
    }
}
