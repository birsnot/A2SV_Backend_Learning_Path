using Blog.Data;
using Blog.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Blog.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public PostsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var posts = await _context.Posts.Include(p => p.Comments).ToListAsync();
            return Ok(posts);
        }

        [HttpGet("{postId:int}")]
        public async Task<IActionResult> Get(int postId)
        {
            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == postId);

            if (post == null)
                return NotFound();

            return Ok(post);
        }

        [HttpPost]
        public async Task<IActionResult> Post(Post post)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _context.Posts.AddAsync(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { postId = post.Id }, post);
        }

        [HttpPut("{postId:int}")]
        public async Task<IActionResult> Put(int postId, Post updatedPost)
        {
            if (postId != updatedPost.Id)
                return BadRequest();

            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
                return NotFound();

            post.Title = updatedPost.Title;
            post.Content = updatedPost.Content;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{postId:int}")]
        public async Task<IActionResult> Delete(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);

            if (post == null)
                return NotFound();

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}