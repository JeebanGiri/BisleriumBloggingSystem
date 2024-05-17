using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class CommentLike
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey(nameof(userFK))]

        public string? UserId { get; set; }

        public bool ReactionType { get; set; }

        public virtual AppUser? userFK { get; set; }

        [ForeignKey(nameof(commentFk))]

        public Guid CommentId { get; set; }

        public virtual Comment? commentFk { get; set; }

    }
}
