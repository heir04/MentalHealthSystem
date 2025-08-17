namespace MentalHealthSystem.Domain.Entities
{
    public class Story : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public bool IsFlagged { get; set; }
        public User? User { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }
}