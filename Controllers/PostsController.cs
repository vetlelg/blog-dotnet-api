using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using blog_dotnet_api.Models;

namespace blog_dotnet_api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly PostContext _context;

        public PostsController(PostContext context)
        {
            _context = context;
        }

        // GET: api/Posts
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostDTO>>> GetPosts()
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            return await _context.Posts
                .Select(x => PostToDTO(x))
                .ToListAsync();
        }

        // GET: api/Posts/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PostDTO>> GetPost(int id)
        {
          if (_context.Posts == null)
          {
              return NotFound();
          }
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }

            return PostToDTO(post);
        }

        // PUT: api/Posts/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPost(int id, PostDTO postDTO)
        {
            if (id != postDTO.Id)
            {
                return BadRequest();
            }

            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }
            post.Content = postDTO.Content;

            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
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

        // POST: api/Posts
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PostDTO>> PostPost(PostDTO postDTO)
        {
          if (_context.Posts == null)
          {
              return Problem("Entity set 'PostContext.Posts'  is null.");
          }

          var post = new Post
          {
            Id = postDTO.Id,
            UserId = postDTO.UserId,
            Content = postDTO.Content,
            Timestamp = DateTime.Now
          };

            _context.Posts.Add(post);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPost), new { id = post.Id }, PostToDTO(post));
        }

        // DELETE: api/Posts/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            if (_context.Posts == null)
            {
                return NotFound();
            }
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PostExists(int id)
        {
            return (_context.Posts?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        private static PostDTO PostToDTO(Post post) =>
            new PostDTO
            {
                Id = post.Id,
                UserId = post.UserId,
                Content = post.Content,
                Timestamp = post.Timestamp
            };
    }
}
