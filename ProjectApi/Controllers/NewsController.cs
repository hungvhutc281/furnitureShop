using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dto;
using ProjectApi.Model;
using FurnitureStore.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private readonly FurnitureStoreContext _context;

        public NewsController(FurnitureStoreContext context)
        {
            _context = context;
        }

        // GET: api/News
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetNews()
        {
            var news = await _context.News
                .Select(n => new NewsDto
                {
                    NewsID = n.NewsID,
                    Title = n.Title,
                    Content = n.Content,
                    Image = n.Image,
                    PostedDate = n.PostedDate,
                    Author = n.Author
                })
                .ToListAsync();

            return Ok(news);
        }

        // GET: api/News/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NewsDto>> GetNews(int id)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            var newsDto = new NewsDto
            {
                NewsID = news.NewsID,
                Title = news.Title,
                Content = news.Content,
                Image = news.Image,
                PostedDate = news.PostedDate,
                Author = news.Author
            };

            return Ok(newsDto);
        }

        // GET: api/News/latest
        [HttpGet("latest")]
        public async Task<ActionResult<IEnumerable<NewsDto>>> GetLatestNews()
        {
            var news = await _context.News
                .Select(n => new NewsDto
                {
                    NewsID = n.NewsID,
                    Title = n.Title,
                    Content = n.Content,
                    Image = n.Image,
                    PostedDate = n.PostedDate,
                    Author = n.Author
                })
                .OrderByDescending(n => n.PostedDate)
                .Take(3)
                .ToListAsync();

            return Ok(news);
        }

        // POST: api/News
        [HttpPost]
        public async Task<ActionResult<NewsDto>> CreateNews(CreateNewsDto createNewsDto)
        {
            var news = new News
            {
                Title = createNewsDto.Title,
                Content = createNewsDto.Content,
                Image = createNewsDto.Image,
                PostedDate = DateTime.Now,
                Author = createNewsDto.Author
            };

            _context.News.Add(news);
            await _context.SaveChangesAsync();

            var newsDto = new NewsDto
            {
                NewsID = news.NewsID,
                Title = news.Title,
                Content = news.Content,
                Image = news.Image,
                PostedDate = news.PostedDate,
                Author = news.Author
            };

            return CreatedAtAction(nameof(GetNews), new { id = news.NewsID }, newsDto);
        }

        // PUT: api/News/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateNews(int id, UpdateNewsDto updateNewsDto)
        {
            var news = await _context.News.FindAsync(id);

            if (news == null)
            {
                return NotFound();
            }

            news.Title = updateNewsDto.Title;
            news.Content = updateNewsDto.Content;
            news.Image = updateNewsDto.Image;
            news.Author = updateNewsDto.Author;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NewsExists(id))
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

        // DELETE: api/News/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNews(int id)
        {
            var news = await _context.News.FindAsync(id);
            if (news == null)
            {
                return NotFound();
            }

            _context.News.Remove(news);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NewsExists(int id)
        {
            return _context.News.Any(e => e.NewsID == id);
        }
    }
}