using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.BisleriumBloggingSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentLikeController : ControllerBase
    {
        private readonly ICommentLikeService _commentLikeService;
        public CommentLikeController(ICommentLikeService commentLikeService)
        {
            _commentLikeService = commentLikeService;
        }

        [HttpPost, Route("UpvoteCmt")]
        public async Task<IActionResult> Upvote(CommentLike
           cmtLike)
        {
            var result = await _commentLikeService.AddUpvote(cmtLike);
            return Ok(result);
        }

        [HttpPost, Route("DownvoteCmt")]
        public async Task<IActionResult> DownVote(CommentLike cmtLike)
        {
            var result = await _commentLikeService.AddDownVote(cmtLike);
            return Ok(result);
        }

    }
}
