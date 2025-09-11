namespace MentalHealthSystem.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public User? User { get; set; }
        public Story? Story { get; set; }
        public ICollection<Reaction>? Reactions { get; set; }
    }
}