using MentalHealthSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthSystem.Infrastructure.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FlaggedContent> FlaggedContents { get; set; }
        public DbSet<Story> Story { get; set; }
        public DbSet<Therapist> Therapists { get; set; }
        public DbSet<TherapySession> TherapySessions { get; set; }
        public DbSet<User> Users { get; set; }

    }
}