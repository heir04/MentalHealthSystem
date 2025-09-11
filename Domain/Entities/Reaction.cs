namespace MentalHealthSystem.Domain.Entities
{
    public class Reaction : BaseEntity
    {
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;
        
        public Guid? StoryId { get; set; }
        public Story? Story { get; set; }
        
        public Guid? CommentId { get; set; }
        public Comment? Comment { get; set; }
        
        public string Type { get; set; } = "Like"; // Like, Love, Laugh, Sad, etc.
    }
}