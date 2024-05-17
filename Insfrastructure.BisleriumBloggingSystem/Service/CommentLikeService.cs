using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Insfrastructure.BisleriumBloggingSystem.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.BisleriumBloggingSystem.Service
{
    public class CommentLikeService : ICommentLikeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CommentLikeService(ApplicationDBContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<CommentLike> GetUserCommentsLike(Guid id, string u_id)
        {
            var res = await _dbContext.CommentLike.FirstOrDefaultAsync(x => x.CommentId == id && x.UserId == u_id);
            return res;

        }

        public async Task<BlogCommentResponse> AddDownVote(CommentLike cmtLike)
        {
            if (cmtLike == null || cmtLike.ReactionType)
                return new BlogCommentResponse(false, "Invalid operation: like is null or it's an upvote");

            var commentService = new CommentService(_dbContext);

            var existingLike = await GetUserCommentsLike(cmtLike.CommentId, cmtLike.UserId);
            var comments = await commentService.GetCommentById(cmtLike.CommentId);

            if (existingLike != null)
            {
                if (!existingLike.ReactionType)
                {
                    _dbContext.CommentLike.Remove(existingLike);
                    comments.Total_Dislike--;

                    _dbContext.Comments.Update(comments);
                    await _dbContext.SaveChangesAsync();
                    return new BlogCommentResponse(true, "Downvote removed successfully", cmtLike);
                }
                else
                {
                    return new BlogCommentResponse(false, "Cannot downvote an upvoted post");
                }
            }
            else
            {
                _dbContext.CommentLike.Add(cmtLike);
                comments.Total_Like++;

                _dbContext.Comments.Update(comments);
                await _dbContext.SaveChangesAsync();
                return new BlogCommentResponse(true, "Downvote added successfully", cmtLike);
            }


        }
        public async Task<BlogCommentResponse> AddUpvote(CommentLike cmtLike)
        {
            if (cmtLike == null || !cmtLike.ReactionType)
                return new BlogCommentResponse(false, "Invalid operation: like is null or it's an downvote");

            var commentService = new CommentService(_dbContext);
            var existingLike = await GetUserCommentsLike(cmtLike.CommentId, cmtLike.UserId);
            var comments = await commentService.GetCommentById(cmtLike.CommentId);

            if (existingLike != null)
            {
                if (existingLike.ReactionType)
                {
                    _dbContext.CommentLike.Remove(existingLike);
                    comments.Total_Like--;

                    _dbContext.Comments.Update(comments);
                    await _dbContext.SaveChangesAsync();
                    return new BlogCommentResponse(true, "Upvote removed successfully", cmtLike);
                }
                else
                {
                    return new BlogCommentResponse(false, "Cannot downvote an upvoted post");
                }
            }
            else
            {
                _dbContext.CommentLike.Add(cmtLike);
                comments.Total_Like++;

                _dbContext.Comments.Update(comments);
                await _dbContext.SaveChangesAsync();
                return new BlogCommentResponse(true, "Upvote added successfully", cmtLike);
            }


        }

        public async Task DeleteVote(Guid id)
        {
            var like = await _dbContext.BlogLike.FirstOrDefaultAsync(x => x.Id == id);
            if (like != null)
            {
                _dbContext.BlogLike.Remove(like);
                await _dbContext.SaveChangesAsync();
            }
        }

    }
}
