using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Insfrastructure.BisleriumBloggingSystem.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogLikeController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IBlogLikeService _blogLikeService;

        public BlogLikeController(UserManager<AppUser> userManager, IBlogLikeService blogLikeService)
        {
            _userManager = userManager;
            _blogLikeService = blogLikeService;
        }

        [Authorize(Roles = "Blogger")]
        [HttpPost, Route("upvote")]
        public async Task<IActionResult> Upvote([FromBody] BlogLike blogLike)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            var result = await _blogLikeService.AddUpvote(blogLike, user.Id);
            return Ok(result);
        }

        [Authorize(Roles = "Blogger")]
        [HttpPost, Route("downvote")]
        public async Task<IActionResult> DownVote(BlogLike blogLike)
        {
            var token = HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var user = await _userManager.FindByEmailAsync(User.FindFirst(ClaimTypes.Email)?.Value);
            var result = await _blogLikeService.AddDownVote(blogLike, user.Id);
            return Ok(result);
        }

        [HttpGet, Route("get-users-likes")]
        public async Task<IActionResult> GetUsersLike(Guid id, string u_id)
        {
            try
            {
                var blogLike = await _blogLikeService.GetUsersLike(id, u_id);
                return Ok(blogLike);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve blogs");
            }
        }

        [HttpGet, Route("get-total-likes/{id}")]
        public async Task<IActionResult> GetTotalLike(Guid id)
        {
            try
            {
                var totalLikes = await _blogLikeService.GetTotalLikes(id);
                return Ok(totalLikes);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve total likes");
            }
        }


        [HttpGet, Route("get-users-dislikes")]
        public async Task<IActionResult> GetUsersDisLike(Guid id, string u_id)
        {
            try
            {
                var blogLike = await _blogLikeService.GetUsersDisLike(id, u_id);
                return Ok(blogLike);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to retrieve blogs");
            }
        }

    }
}
