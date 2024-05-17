using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]

    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogService _blogService;

        public BlogController(UserManager<AppUser> userManager,IBlogService blogService)
        {
            _userManager = userManager;
            _blogService = blogService;
        }

        [Authorize(Roles = "Blogger")]
        [HttpPost, Route("create-blog")]
        public async Task<IActionResult> AddBlog([FromForm] Blog blog, [FromForm] IFormFile? imageFile)
        {
            try
            {

                var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);

                var createdBlog = await _blogService.CreateBlog(blog, imageFile, user.Id);
                Console.WriteLine(imageFile);

                // Check if the blog was created successfully
                if (createdBlog != null)
                {
                    // Return the created blog along with a success message
                    return Ok(new { blog = createdBlog, message = "Blog created successfully." });
                }
                else
                {
                    // If the blog creation failed, return a 500 Internal Server Error
                    return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create blog.");
                }
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create Blog");
            }
        }



        [Authorize(Roles = "Blogger")]
        [HttpPost, Route("update-blog")]
        public async Task<IActionResult> UpdateBlog(Blog blog)
        {
            try
            {
                var updateBlog = await _blogService.UpdateBlog(blog);
                return Ok(updateBlog);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to update blog");
            }
        }

        [Authorize(Roles = "Blogger, Admin")]
        [HttpDelete, Route("delete-blog/{id}")]
        public async Task<IActionResult> DeleteBlog(Guid id)
        {
            try
            {
                await _blogService.DeleteBlog(id);
                return Ok("Blog deleted successfully");
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to delete blog");
            }
        }

        [HttpGet, Route("get-blog")]
        public async Task<IActionResult> GetAllBlogs()
        {
            try
            {
                var blogs = await _blogService.GetAllBlog();
                return Ok(blogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve blogs");
            }
        }

        [Authorize(Roles = "Blogger, Admin")]
        [HttpGet, Route("get-all-blog")]
        public async Task<IActionResult> GetAllBloggerBlogs()
        {
            try
            {
                //var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                //var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);

                // Retrieve the userId of the logged-in user
                var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                Console.WriteLine(userId);

                // Call the service method with the userId
                var blogs = await _blogService.GetAllBloggerBlogs(userId);

                return Ok(blogs);
            }

            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve blogs");
            }
        }

        [HttpGet, Route("get-blog/{id}")]
        public async Task<IActionResult> GetBlogById(Guid id)
        {
            try
            {
                var blogs = await _blogService.GetBlogById(id);
                return Ok(blogs);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve blogs");
            }
        }

        [HttpGet("recent-blog")]
        public async Task<IActionResult> GetMostRecentBlog()
        {
            var mostRecentBlog = await _blogService.GetMostRecentBlog();
            if (mostRecentBlog == null)
            {
                return NotFound(); // Return 404 if no blog found
            }

            return Ok(mostRecentBlog); // Return the most recent blog
        }

        

    }
}
