using FurnitureStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProjectApi.Dto;
using ProjectApi.Model;

namespace ProjectApi.Controllers
{
    [Route("api/[controller]")]
        [ApiController]
    public class CommentController : ControllerBase
    {

        private readonly FurnitureStoreContext _context;

        public CommentController(FurnitureStoreContext context)
        {
            _context = context;
        }

        // GET: api/Comment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CommentDTO>>> GetComments()
        {
            return await _context.Comments
                .Include(c => c.User)
                .Select(c => new CommentDTO
                {
                    CommentId = c.CommentId,
                    UserId = c.UserId,
                    UserName = c.User != null ? c.User.FullName : "Người dùng không tồn tại",
                    Content = c.Content,
                    Rating = c.Rating,
                    CreatedAt = c.CreatedAt
                })
                .OrderByDescending(c => c.CreatedAt)
                .ToListAsync();
        }

        // GET: api/Comment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDTO>> GetComment(int id)
        {
            var comment = await _context.Comments
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.CommentId == id);

            if (comment == null)
            {
                return NotFound();
            }

            return new CommentDTO
            {
                CommentId = comment.CommentId,
                UserId = comment.UserId,
                UserName = comment.User.FullName,
                Content = comment.Content,
                Rating = comment.Rating,
                CreatedAt = comment.CreatedAt
            };
        }

        // POST: api/Comment
        [HttpPost]
        public async Task<ActionResult<CommentDTO>> CreateComment(CreateCommentDTO createCommentDTO)
        {
            var comment = new Comment
            {
                UserId = createCommentDTO.UserId,
                Content = createCommentDTO.Content,
                Rating = createCommentDTO.Rating,
                CreatedAt = DateTime.Now
            };

            _context.Comments.Add(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetComment), new { id = comment.CommentId }, comment);
        }

        // PUT: api/Comment/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, UpdateCommentDTO updateCommentDTO)
        {
            if (id != updateCommentDTO.CommentId)
            {
                return BadRequest();
            }

            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            comment.Content = updateCommentDTO.Content;
            comment.Rating = updateCommentDTO.Rating;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CommentExists(id))
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

        // DELETE: api/Comment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CommentExists(int id)
        {
            return _context.Comments.Any(e => e.CommentId == id);
        }
    }
}
