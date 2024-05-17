using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.BisleriumBloggingSystem.Interface
{
    public interface IBlogService
    {
        Task<Blog> CreateBlog(Blog blog, IFormFile imageFile, string UserId);
        Task<Blog> UpdateBlog(Blog blog);
        Task<IEnumerable<Blog>> GetAllBlog();

        //Task<IEnumerable<Blog>> GetAllBloggerBlog();
        Task<List<Blog>> GetAllBloggerBlogs(string userId);
        Task DeleteBlog(Guid id);
        Task<Blog> GetBlogById(Guid id);

        Task<IEnumerable<Blog>> GetMostRecentBlog();

    }
}
