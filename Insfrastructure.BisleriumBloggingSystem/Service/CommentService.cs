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
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.BisleriumBloggingSystem.Service
{
    public class CommentService : ICommentService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly BlogService _blogService;
        public CommentService(ApplicationDBContext context, UserManager<AppUser> userManager, BlogService blogService)
        {
            _dbContext = context;
            _userManager = userManager;
            _blogService = blogService;
        }

        public async Task<CommentResponse> CreateComment(Comment comment, string? userId)
        {
            if (comment != null && userId != null)
            {

                //Assign user_Id
                comment.AuthorId = userId;
                await _dbContext.Comments.AddAsync(comment);
                await _dbContext.SaveChangesAsync();
                Blog blog = await _blogService.GetBlogById(comment.BlogId);
                Console.WriteLine(blog);
                Console.WriteLine(blog.AuthorId);
                await new FirebaseService(_dbContext, _userManager).SendPushNotifications(blog.AuthorId,
"Comment", $"Your blog's has comment by {userId}");
                // Create a response object with the comment and a success message
                return new CommentResponse(true, "Comment added successfully", comment);
            }
            else
            {
                return new CommentResponse(false, "Failed to Comment.");
            }
        }

        public async Task<IEnumerable<Comment>> GetAllComment()
        {
            var commentDetails = await _dbContext.Comments.ToListAsync();
            return commentDetails;
        }

        public async Task<CommentResponse> UpdateComment(Comment comment)
        {
            var existingComment = await _dbContext.Comments.FindAsync(comment.Id);

            if (existingComment == null)
            {
                throw new Exception("Comment not found");
            }
            existingComment.Content = comment.Content;
            await _dbContext.SaveChangesAsync();

            return new CommentResponse(true, "Comment updated successfully", existingComment);


        }


        public async Task DeleteComment(Guid id)
        {
            var existingComment = await _dbContext.Comments.FindAsync(id);

            if (existingComment == null)
            {
                throw new Exception("Comment not found");
            }

            _dbContext.Remove(existingComment);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Comment> GetCommentById(Guid id)
        {
            // Implement logic to retrieve student by ID from the database
            var comments = await _dbContext.Comments.FindAsync(id);
            if (comments == null)
            {
                throw new Exception("Comment not found");
            }
            return comments;
        }

        public async Task<List<Comment>> GetCommentByBlogId(Guid id)
        {
            var comments = await _dbContext.Comments
                .Where(c => c.BlogId == id)
                .OrderByDescending(c => c.Id)
                .Include(c => c.User)
                .ToListAsync();

            if (comments == null || !comments.Any())
            {
                throw new Exception("No comments found for the specified blog ID");
            }

            return comments;
        }


    }
}
//.Select(c => new CommentUserDTO
// {
//     Content = c.Content,
//     FullName = c.User != null ? c.User.FullName : "Unknown" // Assuming FullName is a property of AppUser
// })