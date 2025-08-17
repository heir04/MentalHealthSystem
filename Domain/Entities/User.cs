using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Domain.Entities
{
    public class User : BaseEntity
    {
        public string? Email { get; set; }
        public string? Username { get; set; }
        public string? HashSalt { get; set; }
        public string? PasswordHash { get; set; }
        public string Role { get; set; } = null!;
        public bool IsActive { get; set; }
        public ICollection<Story>? Stories { get; set; }
        public ICollection<Comment>? Comments { get; set; }
    }
}