using MentalHealthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthSystem.Infrastructure.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FlaggedContent> FlaggedContents { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Story> Story { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<TherapySession> TherapySessions { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed Admin User
            // Password: Admin@123 (hardcoded hash)
            modelBuilder.Entity<User>().HasData(new User
            {
                Id = new Guid("00000000-0000-0000-0000-000000000001"),
                Email = "admin@mentalhealthsystem.com",
                Username = "admin",
                HashSalt = "U8KvVHr2LwBnUXJh0Hqz7Q==",
                PasswordHash = "Xx0c8BqF3p1YwN1/v6K3rGZE8JqE3p1YwN1/v6K3rA==",
                Role = "Admin",
                CreatedOn = new DateTime(2025, 12, 17, 0, 0, 0, DateTimeKind.Utc),
                LastModifiedOn = new DateTime(2025, 12, 17, 0, 0, 0, DateTimeKind.Utc),
                LastModifiedBy = new Guid("00000000-0000-0000-0000-000000000001"),
                IsDeleted = false
            });
        }
    }
}