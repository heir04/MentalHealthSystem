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
}
