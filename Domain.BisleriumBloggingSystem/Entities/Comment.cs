using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class Comment
    {
        [Key]
        public Guid Id { get; set; }
        public string? Content { get; set; }
        public int Total_Like { get; set; } = 0;
        public int Total_Dislike { get; set; } = 0;

        [ForeignKey(nameof(Blog))]
        public Guid BlogId { get; set; }    
                
        [ForeignKey(nameof(User))]
        public string? AuthorId { get; set; }

        public virtual AppUser? User { get; set; }

    }
}
