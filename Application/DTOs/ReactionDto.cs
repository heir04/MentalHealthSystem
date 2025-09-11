namespace MentalHealthSystem.Application.DTOs
{
    public class ReactionDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public Guid? StoryId { get; set; }
        public Guid? CommentId { get; set; }
        public string Type { get; set; } = string.Empty; // Like, Love, Laugh, Sad, etc.
        public DateTime CreatedOn { get; set; }
    }

    public class CreateReactionDto
    {
        public string Type { get; set; } = "Like";
    }

    public class ReactionSummaryDto
    {
        public string Type { get; set; } = string.Empty;
        public int Count { get; set; }
        public bool UserReacted { get; set; }
    }
}
