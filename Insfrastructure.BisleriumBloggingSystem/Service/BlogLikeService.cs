using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Infrastructure.BisleriumBloggingSystem.Service;
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
    public class BlogLikeService : IBlogLikeService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;



        public BlogLikeService(ApplicationDBContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<BlogLike> GetUsersLike(Guid id, string u_id)
        {
            var res = await _dbContext.BlogLike.FirstOrDefaultAsync(x => x.BlogId == id && x.UserId == u_id);
            return res;

        }

        public async Task<int> GetTotalLikes(Guid id)
        {
            var totalLikes = await _dbContext.BlogLike.CountAsync(x => x.BlogId == id);
            return totalLikes;
        }



        public async Task<BlogLike> GetUsersDisLike(Guid id, string u_id)
        {
            var res = await _dbContext.BlogLike.FirstOrDefaultAsync(x => x.BlogId == id && x.UserId == u_id);
            return res;

        }


        public async Task<BlogLikeResponse> AddUpvote(BlogLike blogLike, string userId)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");
            }

            if (blogLike == null || !blogLike.ReactionType)
                return new BlogLikeResponse(false, "Invalid operation: like is null or it's a downvote");

            var blogService = new BlogService(_dbContext, _userManager, _httpContextAccessor);
            var existingLike = await GetUsersLike(blogLike.BlogId, userId);
            var blog = await blogService.GetBlogById(blogLike.BlogId);

            if (existingLike != null)
            {
                if (existingLike.ReactionType)
                {
                    _dbContext.BlogLike.Remove(existingLike);
                    blog.Total_Like--;
                    blog.Popularity = (2 * blog.Total_Like) + (-1 * blog.Total_DisLike) + (1 * blog.Total_Comment);
                    _dbContext.Blogs.Update(blog);
                    await _dbContext.SaveChangesAsync();

                    await new FirebaseService(_dbContext, _userManager).SendPushNotifications(blog.AuthorId,
               "Comment", $"Your blog's reaction has been updated: {blogLike.ReactionType}");
                    return new BlogLikeResponse(true, "Upvote removed successfully");
                }
                else
                {
             //       await new FirebaseService(_dbContext, _userManager).SendPushNotifications(blog.AuthorId,
             //"Comment", $"Your blog's reaction has been updated: {blogLike.ReactionType}");
                    return new BlogLikeResponse(false, "Cannot upvote a downvoted post");
                }
            }
            else
            {
                _dbContext.BlogLike.Add(blogLike);
                blog.Total_Like++;
                blog.Popularity = (2 * blog.Total_Like) + (-1 * blog.Total_DisLike) + (1 * blog.Total_Comment);
                blogLike.UserId = userId;
                _dbContext.Blogs.Update(blog);
                await _dbContext.SaveChangesAsync();
                return new BlogLikeResponse(true, "Upvote added successfully", blogLike);
            }
        }


        public async Task<BlogLikeResponse> AddDownVote(BlogLike blogLike, string userId)
        {
            if (blogLike == null || blogLike.ReactionType)
                return new BlogLikeResponse(false, "Invalid operation: like is null or it's an upvote");

            var blogService = new BlogService(_dbContext, _userManager, _httpContextAccessor);
            var existingLike = await GetUsersLike(blogLike.BlogId, userId);
            var blog = await blogService.GetBlogById(blogLike.BlogId);

            if (existingLike != null)
            {
                if (!existingLike.ReactionType)
                {
                    _dbContext.BlogLike.Remove(existingLike);

                    blog.Total_DisLike--;
                    blogLike.UserId = userId;
                    blog.Popularity = (2 * blog.Total_Like) + (-1 * blog.Total_DisLike) + (1 * blog.Total_Comment);
                    _dbContext.Blogs.Update(blog);
                    await _dbContext.SaveChangesAsync();
                    await new FirebaseService(_dbContext, _userManager).SendPushNotifications(blog.AuthorId,
              "Comment", $"Your blog's reaction has been updated: {blogLike.ReactionType}");
                    return new BlogLikeResponse(true, "Downvote removed successfully", blogLike);
                }
                else
                {
                    return new BlogLikeResponse(false, "Cannot downvote an upvoted post");
                }
            }

            else
            {
                blogLike.UserId = userId;
                _dbContext.BlogLike.Add(blogLike);
                blog.Total_DisLike++;
                blog.Popularity = (2 * blog.Total_Like) + (-1 * blog.Total_DisLike) + (1 * blog.Total_Comment);
                _dbContext.Blogs.Update(blog);
                await _dbContext.SaveChangesAsync();
                await new FirebaseService(_dbContext, _userManager).SendPushNotifications(blog.AuthorId,
              "Comment", $"Your blog's reaction has been updated: {blogLike.ReactionType}");
                return new BlogLikeResponse(true, "Downvote added successfully", blogLike);
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
