using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class Blog
    {   
        [Key]
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public string? Title { get; set; }
        public string? Comments { get; set; }
        public string? Image     { get; set; }

        [NotMapped]
        public IFormFile? ImageFile { get; set; }
        public int Total_Like { get; set; } = 0;
        public int Total_DisLike { get; set; } = 0;

        public int Total_Comment { get; set; } = 0;
        public int Popularity { get; set; }
        public DateTime? CreatedDate { get; set; } = DateTime.Now;
        public DateTime? Updated_At { get; set; }

        [ForeignKey(nameof(User))]
        public string? AuthorId { get; set; }

        public virtual AppUser? User { get; set; }

    }
}
