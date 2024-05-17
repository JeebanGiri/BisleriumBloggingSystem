using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class Dashboard
    {
        public int TotalUserCount { get; set; }
        public int TotalBlogPost { get; set; }
        public int TotalComments { get; set; }
        public int TotalLikes { get; set; }
        public int TotalDislikes { get; set; }
    }
}
