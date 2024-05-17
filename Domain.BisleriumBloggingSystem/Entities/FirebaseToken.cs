using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.BisleriumBloggingSystem.Entities
{
    public class FirebaseToken
    {
        [Key]
        public Guid ID { get; set; }

        public string? Token { get; set; }

        // User foreign Key
        [ForeignKey(nameof(User))]
        public string? UserID { get; set; }
        public virtual AppUser? User { get; set; }


    }
}
