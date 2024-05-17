using Application.BisleriumBloggingSystem.Interface;
using Domain.BisleriumBloggingSystem.Entities;
using Domain.BisleriumBloggingSystem.Entities.Statics;
using Insfrastructure.BisleriumBloggingSystem.Config;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Insfrastructure.BisleriumBloggingSystem.Service
{
    public class TopTenService
    {
        private readonly ApplicationDBContext _context;

        //public TopTenService()
        //{
        //    _context = new ApplicationDBContext();
        //}

        //public async Task<TopTen> GetTopTenData(int year, int month)
        //{
        //    TopTen topTen = new();

        //    // get top ten blog of all time 
        //    topTen.AllTimeBlog = await GetTopTenBlogAllTime();

        //    // get top ten blogger of all time
        //    topTen.AllTimeBlogger = await GetTopTenBloggerAllTime();

        //    // get top ten blog of selected month
        //    topTen.MonthlyTopBlog = await GetBlogsOfMonth(year, month);

        //    // get top ten blogger of selected month
        //    topTen.MonthlyTopBlogger = await GetBloggerOfMonth(year, month);

        //    return topTen;
        //}

        //private async Task<List<Blog>> GetTopTenBlogAllTime()
        //{
        //    return await _context.Blogs.OrderByDescending(x => x.Popularity).OrderBy(x => x.Title).Take(10).ToListAsync();
        //}

        //private async Task<List<AppUser>> GetTopTenBloggerAllTime()
        //{
        //    var bloggerPopularity = await _context.Blogs
        //       .Include(x => x.User)
        //       .GroupBy(x => x.User)
        //       .Select(g => new { Blogger = g.Key, TotalPopularity = g.Sum(x => x.Popularity) })
        //       .OrderByDescending(x => x.TotalPopularity)
        //       .Take(10)
        //       .ToListAsync();

        //    var topTenBloggers = bloggerPopularity.Select(x => x.Blogger).ToList();

        //    return topTenBloggers;

        //}

        //private async Task<List<Blog>> GetBlogsOfMonth(int year, int month)
        //{
        //    Dictionary<Blog, int> blogPopularity = await PopularityOfBlog(year, month);

        //    var blogsWithPopularity = blogPopularity.Select(kv => new { Blog = kv.Key, Popularity = SetStatics.CalculatePopularity(kv.Key.UpVotes, kv.Key.DownVotes, kv.Key.Comments) });

        //    var topTenBlogs = blogsWithPopularity.OrderByDescending(kv => kv.Popularity)
        //                                         .Select(kv => kv.Blog)
        //                                         .Take(10)
        //                                         .ToList();

        //    return topTenBlogs;
        //}

        //private async Task<List<AppUser>> GetBloggerOfMonth(int year, int month)
        //{
        //    Dictionary<Blog, int> blogPopularity = await PopularityOfBlog(year, month);

        //    var bloggerPopularity = blogPopularity
        //        .GroupBy(bp => bp.Key.User)
        //        .Select(group => new
        //        {
        //            Blogger = group.Key,
        //            TotalPopularity = group.Sum(bp => bp.Value)
        //        });

        //    var topTenBloggers = bloggerPopularity.OrderByDescending(bp => bp.TotalPopularity)
        //                                          .Select(bp => bp.Blogger)
        //                                          .Take(10)
        //                                          .ToList();

        //    return topTenBloggers;
        //}

        //private async Task<Dictionary<Blog, int>> PopularityOfBlog(int year, int month)
        //{
        //    List<Blog> blogs = await _context.Blogs.Where(x => x.CreatedDate.Year == year && x.CreatedDate.Month == month).ToListAsync();

        //    List<BlogLike> likeBlogs = await _context.BlogLike
        //        .Include(x => x.BlogId)
        //        .Include(x => x.BlogId.User)
        //        .Where(x => x.CreatedAt.Year == year && x.CreatedAt.Month == month)
        //        .ToListAsync();

        //    List<Comment> commentBlogs = await _context.Comments
        //        .Include(x => x.BlogId)
        //        .Include(x => x.BlogId.User)
        //        .Where(x => x.CreatedAt.Year == year && x.CreatedAt.Month == month)
        //        .ToListAsync();

        //    Dictionary<Blog, int> blogPopularity = new Dictionary<Blog, int>();

        //    foreach (var likeBlog in likeBlogs)
        //    {
        //        if (!blogPopularity.ContainsKey(likeBlog.BlogId))
        //        {
        //            blogPopularity[likeBlog.BlogId] = 0;
        //        }

        //        switch (likeBlog.ReactionType)
        //        {
        //            case ReactionType.UpVote:
        //                blogPopularity[likeBlog.BlogId] += SetStatics.Upvote;
        //                break;
        //            case ReactionType.DownVote:
        //                blogPopularity[likeBlog.BlogId] += SetStatics.Downvote;
        //                break;
        //        }
        //    }

        //    foreach (var commentBlog in commentBlogs)
        //    {
        //        if (!blogPopularity.ContainsKey(commentBlog.BlogId))
        //        {
        //            blogPopularity[commentBlog.BlogId] = 0;
        //        }
        //        blogPopularity[commentBlog.BlogId] += SetStatics.Comment;
        //    }

        //    foreach (var blog in blogs)
        //    {
        //        if (!blogPopularity.ContainsKey(blog))
        //        {
        //            blogPopularity[blog] = 0;
        //        }
        //    }

        //    return blogPopularity;
        //}
    }
}
