using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class BlogLike
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(userFK))]

        public string? UserId { get; set; }
        public virtual AppUser? userFK { get; set; }

        public bool ReactionType { get; set; }


        [ForeignKey(nameof(blogFK))]

        public Guid BlogId { get; set; }

        public virtual Blog? blogFK { get; set; }
    }
}
