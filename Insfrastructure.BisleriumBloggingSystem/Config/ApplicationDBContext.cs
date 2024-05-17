using Domain.BisleriumBloggingSystem.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Insfrastructure.BisleriumBloggingSystem.Config
{
    public class ApplicationDBContext : IdentityDbContext<AppUser>
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=desktop-6qdoe18; Database=BisleriumSystemDb;Trusted_Connection=True;TrustServerCertificate=True;MultipleActiveResultSets=True;");
        }
        public DbSet<Blog> Blogs { get; set; }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<BlogLike> BlogLike { get; set; }

        public DbSet<CommentLike> CommentLike { get; set; }

        public DbSet<BlogHistory> BlogHistories { get; set; }

        public DbSet<CommentHistory> CommentHistories { get; set; }

        public DbSet<FirebaseToken> FirebaseToken { get; set; }



    }
}

