using Domain.BisleriumBloggingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface ICommentLikeService
    {
        Task<BlogCommentResponse> AddUpvote(CommentLike likecmt);
        Task<BlogCommentResponse> AddDownVote(CommentLike likecmt);
        Task<CommentLike> GetUserCommentsLike(Guid id, string u_id);
        Task DeleteVote(Guid id);
    }
}
