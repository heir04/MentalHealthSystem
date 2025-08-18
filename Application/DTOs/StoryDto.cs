namespace MentalHealthSystem.Application.DTOs
{
    public class StoryDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UserName { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public List<ReactionDto>? Reactions { get; set; }
    }

    public class CreateStoryDto
    {
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateStoryDto
    {
        public string Content { get; set; } = string.Empty;
    }
}
