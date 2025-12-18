namespace MentalHealthSystem.Application.DTOs
{
    public class CommentDto
    {
        public Guid Id { get; set; }
        public Guid StoryId { get; set; }
        public Guid UserId { get; set; }
        public string? Content { get; set; }
        public int LikesCount { get; set; }
        public DateTime CreatedOn { get; set; }
        public string? UserName { get; set; } 
    }

    public class CreateCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateCommentDto
    {
        public string Content { get; set; } = string.Empty;
    }
}
