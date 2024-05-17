using Domain.BisleriumBloggingSystem.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface IUserService
    {
        public Task<IEnumerable<AppUser>> GetAllAdmin();
        public Task<IEnumerable<AppUser>> GetAllBloggers();
        public Task<IEnumerable<AppUser>> GetAllBloggersByPopularity(int month);
        public Task<AppUser?> GetCurrentUser();


    }
}
