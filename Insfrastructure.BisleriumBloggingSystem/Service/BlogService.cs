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
    public class BlogService : IBlogService
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BlogService(ApplicationDBContext context, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _dbContext = context;
            _userManager = userManager;
        }

        public async Task<Blog> CreateBlog(Blog blog, IFormFile imageFile, string userId)
        {
            try
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
                    var filePath = Path.Combine("wwwroot", "images", fileName); // Adjust this path according to your project structure

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await imageFile.CopyToAsync(fileStream);
                    }

                    blog.Image = fileName; // Set the image file name in the blog entity
                    blog.AuthorId = userId;
                }

                await _dbContext.Blogs.AddAsync(blog);
                await _dbContext.SaveChangesAsync();
                return blog;
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine(ex.Message);
                throw; // Rethrow the exception to the caller
            }

        }

        public async Task<IEnumerable<Blog>> GetAllBlog()
        {

            var blogDetails = await _dbContext.Blogs
            .Include(b => b.User) // Include the related User entity
            .ToListAsync();

            return blogDetails;
        }

        public async Task<Blog> UpdateBlog(Blog blog)
        {
            Blog prevBlog = await GetBlogById(blog.Id);
            BlogHistory bloghistory = new BlogHistory();


            if (prevBlog != null)
            {
                bloghistory.Blog = prevBlog.Id;
                bloghistory.PreviousBlogTitle = prevBlog.Title;
                bloghistory.PreviousBlogContent = prevBlog.Content;
                bloghistory.BlogCreatedDateTime = prevBlog.CreatedDate;
                bloghistory.PreviousBlogImage = prevBlog.Image;
                await _dbContext.BlogHistories.AddAsync(bloghistory);

                if (!string.IsNullOrEmpty(blog.Image))
                {
                    prevBlog.Image = blog.Image;

                }
                prevBlog.Title = blog.Title;
                prevBlog.Content = blog.Content;


                _dbContext.Blogs.Update(prevBlog);
                await _dbContext.SaveChangesAsync();

            }
            return prevBlog;

        }

        public async Task DeleteBlog(Guid id)
        {
            var existingBlog = await _dbContext.Blogs.FindAsync(id);

            if (existingBlog == null)
            {
                throw new Exception("Blog not found");
            }

            _dbContext.Remove(existingBlog);

            await _dbContext.SaveChangesAsync();
        }

        public async Task<Blog> GetBlogById(Guid id)
        {
            // Implement logic to retrieve student by ID from the database

            //var blogs = await _dbContext.Blogs.FindAsync(id);
            var blogs = await _dbContext.Blogs
            .Include(b => b.User) // Include the related User entity
            .FirstOrDefaultAsync(b => b.Id == id);
            if (blogs == null)
            {
                throw new Exception("Blog not found");
            }
            //return new List<Blog> { blogs };
            return blogs;
        }

        public async Task<IEnumerable<Blog>> GetMostRecentBlog()
        {
            // Query the database to get the most recent blog
            var mostRecentBlog = await _dbContext.Blogs
                .OrderByDescending(b => b.CreatedDate)
                .Take(5) // Adjust this to the number of most recent blogs you want to retrieve
                .ToListAsync();

            return mostRecentBlog;
        }

        public async Task<List<Blog>> GetAllBloggerBlogs(string userId)
        {
            // Implement logic to retrieve blogs by userId from the database
            var blogs = await _dbContext.Blogs
                .Where(blog => blog.AuthorId == userId)
                .ToListAsync();

            if (blogs == null)
            {
                throw new Exception("No blogs found for the user");
            }

            return blogs;
        }


    }
}