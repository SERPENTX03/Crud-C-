using Microsoft.AspNetCore.Mvc;
using MyMongoApi.Models;
using MyMongoApi.Services;

namespace MyMongoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly PostService _postService;

        public PostController(PostService postService)
        {
            _postService = postService;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<List<Post>> Get() => await _postService.GetAsync();

        // GET: api/Post/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> Get(string id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();
            return post;
        }

        [HttpPost]
        public async Task<IActionResult> Create(Post post)
        {
            await _postService.CreateAsync(post);
            return CreatedAtAction(nameof(Get), new { id = post.Id }, post);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, Post post)
        {
            var existingPost = await _postService.GetByIdAsync(id);
            if (existingPost == null) return NotFound();

            post.Id = id;
            await _postService.UpdateAsync(id, post);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var post = await _postService.GetByIdAsync(id);
            if (post == null) return NotFound();

            await _postService.DeleteAsync(id);
            return NoContent();
        }
    }
}
