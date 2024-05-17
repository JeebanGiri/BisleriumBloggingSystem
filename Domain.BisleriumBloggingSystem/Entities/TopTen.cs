using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class TopTen
    {
        public List<Blog> AllTimeBlog { get; set; }
        public List<AppUser> AllTimeBlogger { get; set; }

        public List<Blog> MonthlyTopBlog { get; set; }
        public List<AppUser> MonthlyTopBlogger { get; set; }

        public TopTen()
        {
            AllTimeBlog = new List<Blog>();
            AllTimeBlogger = new List<AppUser>();
            MonthlyTopBlog = new List<Blog>();
            MonthlyTopBlogger = new List<AppUser>();
        }
    }


}
