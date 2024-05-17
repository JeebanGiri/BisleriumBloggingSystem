using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class BlogLikeResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public BlogLike Like { get; set; }

        public BlogLikeResponse(bool success, string message, BlogLike like = null)
        {
            Success = success;
            Message = message;
            Like = like;
        }
    }
}
