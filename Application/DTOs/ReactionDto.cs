namespace MentalHealthSystem.Application.DTOs
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Type { get; set; } // e.g., Like, Dislike, etc.
        public DateTime CreatedAt { get; set; }
    }

    public class CreateReactionDto
    {
        public Guid StoryId { get; set; }
        public string Type { get; set; } = string.Empty;
    }

    public class UpdateReactionDto
    {
        public string Type { get; set; } = string.Empty;
    }
}
