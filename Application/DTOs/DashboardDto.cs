namespace MentalHealthSystem.Application.DTOs
{
    public class UserDashboardDto
    {
        public UserDto? User { get; set; }
        public int TotalStories { get; set; }
        public int TotalComments { get; set; }
        public int TotalReactionsReceived { get; set; }
        public int UpcomingSessionsCount { get; set; }
        public List<StoryDto>? RecentStories { get; set; }
        public List<TherapySessionDto>? UpcomingSessions { get; set; }
    }

    public class TherapistDashboardDto
    {
        public TherapistDto? Therapist { get; set; }
        public int TotalSessions { get; set; }
        public int PendingSessions { get; set; }
        public int ScheduledSessions { get; set; }
        public int CompletedSessions { get; set; }
        public int CancelledSessions { get; set; }
        public int TotalClients { get; set; }
        public int SessionsThisMonth { get; set; }
        public List<TherapySessionDto>? UpcomingSessions { get; set; }
        public List<TherapySessionDto>? RecentSessions { get; set; }
    }

    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }
        public int TotalStories { get; set; }
        public int TotalComments { get; set; }
        public int TotalReactions { get; set; }
        public int TotalTherapists { get; set; }
        public int ApprovedTherapists { get; set; }
        public int PendingTherapists { get; set; }
        public int TotalSessions { get; set; }
        public int PendingFlaggedContent { get; set; }
        public int NewUsersThisMonth { get; set; }
        public int NewStoriesThisMonth { get; set; }
        public List<UserDto>? RecentUsers { get; set; }
        public List<FlaggedContentDto>? PendingReports { get; set; }
    }

    public class ReportsDashboardDto
    {
        public int TotalFlaggedContent { get; set; }
        public int PendingReview { get; set; }
        public int ReviewedContent { get; set; }
        public int FlaggedStories { get; set; }
        public int FlaggedComments { get; set; }
        public int ContentRemovedThisMonth { get; set; }
        public List<FlaggedContentDto>? RecentFlags { get; set; }
        public Dictionary<string, int>? FlagReasonBreakdown { get; set; }
    }

    public class GeneralDashboardDto
    {
        public int TotalStories { get; set; }
        public int TotalComments { get; set; }
        public int TotalReactions { get; set; }
        public int TotalUsers { get; set; }
        public List<StoryWithStatsDto>? Stories { get; set; }
    }

    public class StoryWithStatsDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? UserName { get; set; }
        public string? Content { get; set; }
        public DateTime CreatedOn { get; set; }
        public int CommentCount { get; set; }
        public int ReactionCount { get; set; }
        public List<CommentDto>? Comments { get; set; }
        public Dictionary<string, int>? ReactionBreakdown { get; set; }
    }
}
