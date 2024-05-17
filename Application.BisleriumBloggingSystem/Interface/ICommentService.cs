using Domain.BisleriumBloggingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface ICommentService
    {
        Task<CommentResponse> CreateComment(Comment comment, string userId);
        Task<IEnumerable<Comment>> GetAllComment();
        Task<CommentResponse> UpdateComment(Comment comment);
        Task DeleteComment(Guid id);
        Task<Comment> GetCommentById(Guid id);

        Task<List<Comment>> GetCommentByBlogId(Guid id);
    }
}
