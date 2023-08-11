using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CommentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var comments = await _context.Comments.ToListAsync();
            return Ok(comments);
        }

        [HttpGet("{commentId:int}")]
        public async Task<IActionResult> Get(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
                return NotFound();

            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Comment comment)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Comments.AddAsync(comment);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { commentId = comment.Id }, comment);
        }

        [HttpPut("{commentId:int}")]
        public async Task<IActionResult> Put(int commentId, Comment updatedComment)
        {
            if (commentId != updatedComment.Id)
                return BadRequest();

            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
                return NotFound();

            comment.Text = updatedComment.Text;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{commentId:int}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);

            if (comment == null)
                return NotFound();

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}