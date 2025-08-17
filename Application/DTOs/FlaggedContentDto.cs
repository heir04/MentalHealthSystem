namespace MentalHealthSystem.Application.DTOs
{
    public class FlaggedContentDto
    {
        public Guid Id { get; set; }
        public Guid StoryId { get; set; }
        public Guid CommentId { get; set; }
        public Guid ReportedByUserId { get; set; }
        public string? Reason { get; set; }
        public DateTime FlaggedAt { get; set; }
        public bool IsReviewed { get; set; }
        public string? AdminResponse { get; set; }
        public string? ReportedByUserName { get; set; } // Optionally include related user info
    }

    public class CreateFlaggedContentDto
    {
        public Guid StoryId { get; set; }
        public Guid CommentId { get; set; }
        public string Reason { get; set; } = string.Empty;
    }

    public class UpdateFlaggedContentDto
    {
        public bool IsReviewed { get; set; }
        public string? AdminResponse { get; set; }
    }
}
