using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class CommentHistory
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string? PreviousCommentContent { get; set; }
        public DateTime? CommentCreatedDateTime { get; set; }
        public DateTime? CommentModifiedDateTime { get; set; } = DateTime.Now;

        [ForeignKey(nameof(commentFK))]

        public Guid Comments { get; set; }


        public virtual Comment? commentFK { get; set; }
    }
}
