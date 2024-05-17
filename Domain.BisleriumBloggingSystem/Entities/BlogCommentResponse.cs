using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class BlogCommentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public CommentLike Likecmt { get; set; }

        public BlogCommentResponse(bool success, string message, CommentLike like = null)
        {
            Success = success;
            Message = message;
            Likecmt = like;
        }

    }
}
