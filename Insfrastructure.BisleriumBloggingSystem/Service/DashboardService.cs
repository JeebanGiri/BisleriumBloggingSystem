using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Insfrastructure.BisleriumBloggingSystem.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.BisleriumBloggingSystem.Service
{
    public class DashboardService : IDashboardService
    {
        private readonly ApplicationDBContext _dbContext;
        public DashboardService(ApplicationDBContext context)
        {
            _dbContext = context;
        }

        public async Task<Dashboard> GetBlogStats()
        {
            var blogStats = new Dashboard();
            blogStats.TotalUserCount = await _dbContext.Users.CountAsync();
            blogStats.TotalBlogPost = await _dbContext.Blogs.CountAsync();
            blogStats.TotalLikes = await _dbContext.Blogs.SumAsync(b => b.Total_Like);
            blogStats.TotalDislikes = await _dbContext.Blogs.SumAsync(b => b.Total_DisLike);
            blogStats.TotalComments = await _dbContext.Comments.CountAsync(); // Assuming you have a Comments DbSet in your context

            return blogStats;
        }
    }
}
