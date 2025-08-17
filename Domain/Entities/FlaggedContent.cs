namespace MentalHealthSystem.Domain.Entities
{
    public class FlaggedContent : BaseEntity
    {
        public Guid StoryId { get; set; }     
        public Guid CommentId { get; set; }
        public Guid ReportedByUserId { get; set; }
        public string? Reason { get; set; }
        public bool IsReviewed { get; set; } = false;
        public string? AdminResponse { get; set; }

        public User? ReportedByUser { get; set; }

    }
}