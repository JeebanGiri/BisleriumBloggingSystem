using Domain.BisleriumBloggingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface IBlogLikeService
    {
        Task<BlogLikeResponse> AddUpvote(BlogLike like, string userId);
        Task<BlogLikeResponse> AddDownVote(BlogLike like, string userId);
        Task<BlogLike> GetUsersLike(Guid id, string u_id);

        Task<BlogLike> GetUsersDisLike(Guid id, string u_id);
        Task DeleteVote(Guid id);
        Task<int> GetTotalLikes(Guid id);
    }
}
