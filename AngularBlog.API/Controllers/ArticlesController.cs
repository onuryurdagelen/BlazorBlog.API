using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BlazorBlog.API.Models;
using BlazorBlog.API.Response;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Drawing.Printing;
using System.Globalization;
using BlazorBlog.API.Dtos;

namespace BlazorBlog.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly AngularBlogDBContext _context;

        public ArticlesController(AngularBlogDBContext context)
        {
            _context = context;
        }

        // GET: api/Articles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticles()
        {
            Thread.Sleep(millisecondsTimeout: 2000);
            return await _context.Articles.ToListAsync();
        }
        [HttpGet("{page}/{pageSize}")]
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArticlesByPaginately(int page= 1,int pageSize = 5)
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
        public async Task<ActionResult<Article>> GetArticle(int id)
        {
            var article = await _context.Articles.Include(x => x.Category).Where(x => x.Id == id).FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound();
            }

            return article;
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
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArticlesByCategoryId(int categoryId,int page = 1,int pageSize = 5) {

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
        public async Task<IEnumerable<Article>> GetMostViewedArticles()
        {
            IQueryable<Article> query;
            query = _context.Articles.OrderByDescending(x => x.ViewCount).Take(5);
            return await query.ToListAsync();
        }
        [HttpGet("ArchivesArticles/{tillTheYears:int}")]
        public  IActionResult GetArchivesArticles(int tillTheYears = 5)
        {
            var query = _context.Articles.Where(x => x.PublishDate >=DateTime.Now.AddYears(-(tillTheYears))).GroupBy(x => new
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
            
            return Ok(query);
        }
        [HttpGet("ListOfArchivedArticles/{year}/{month}/{page}/{pageSize}")]
        public async Task<ActionResult<ServerDataResponse<ArticleDto>>> GetArchivedArticlesByYearAndMonth(int year,int month,int page = 1, int pageSize = 5)
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
    }
}
