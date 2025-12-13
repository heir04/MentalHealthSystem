using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidatorHelper _validatorHelper;

        public DashboardService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper)
        {
            _unitOfWork = unitOfWork;
            _validatorHelper = validatorHelper;
        }

        public async Task<BaseResponse<UserDashboardDto>> GetUserDashboard()
        {
            var response = new BaseResponse<UserDashboardDto>();
            var userId = _validatorHelper.GetUserId();

            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            var user = await _unitOfWork.User.Get(u => u.Id == userId && !u.IsDeleted);
            if (user == null)
            {
                response.Message = "User not found";
                return response;
            }

            // Get user statistics
            var stories = await _unitOfWork.Story.GetAll(s => s.UserId == userId && !s.IsDeleted);
            var comments = await _unitOfWork.Comment.GetAll(c => c.UserId == userId && !c.IsDeleted);
            
            // Get reactions received on user's stories
            var userStoryIds = stories.Select(s => s.Id).ToList();
            var reactionsReceived = 0;
            foreach (var storyId in userStoryIds)
            {
                var storyReactions = await _unitOfWork.Reaction.GetReactionsByStoryAsync(storyId);
                reactionsReceived += storyReactions.Count();
            }

            // Get upcoming sessions
            var upcomingSessions = await _unitOfWork.TherapySession.GetAll(ts => 
                ts.UserId == userId && 
                (ts.Status == TherapySessionStatus.Pending || ts.Status == TherapySessionStatus.Scheduled));

            // Get recent stories
            var recentStories = await _unitOfWork.Story.GetAllStory(s => s.UserId == userId && !s.IsDeleted);

            response.Data = new UserDashboardDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,
                    IsActive = user.IsActive,
                    CreatedOn = user.CreatedOn
                },
                TotalStories = stories.Count(),
                TotalComments = comments.Count(),
                TotalReactionsReceived = reactionsReceived,
                UpcomingSessionsCount = upcomingSessions.Count(),
                RecentStories = recentStories.Take(5).Select(s => new StoryDto
                {
                    Id = s.Id,
                    UserId = s.UserId,
                    UserName = s.User?.Username,
                    Content = s.Content,
                    CreatedOn = s.CreatedOn
                }).ToList(),
                UpcomingSessions = upcomingSessions.Take(5).Select(ts => new TherapySessionDto
                {
                    Id = ts.Id,
                    UserId = ts.UserId,
                    TherapistId = ts.TherapistId,
                    Status = ts.Status.ToString()
                }).ToList()
            };

            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<AdminDashboardDto>> GetAdminDashboard()
        {
            var response = new BaseResponse<AdminDashboardDto>();
            var userRole = _validatorHelper.GetUserRole();

            if (userRole != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            // Get all statistics
            var allUsers = await _unitOfWork.User.GetAll(u => !u.IsDeleted);
            var allStories = await _unitOfWork.Story.GetAll(s => !s.IsDeleted);
            var allComments = await _unitOfWork.Comment.GetAll(c => !c.IsDeleted);
            var allReactions = await _unitOfWork.Reaction.GetAll(r => !r.IsDeleted);
            var allTherapists = await _unitOfWork.Therapist.GetAll(t => !t.IsDeleted);
            var allSessions = await _unitOfWork.TherapySession.GetAll(ts => !ts.IsDeleted);
            var allFlaggedContent = await _unitOfWork.FlaggedContent.GetAll(fc => !fc.IsDeleted);

            // Get monthly statistics
            var newUsersThisMonth = allUsers.Count(u => u.CreatedOn >= startOfMonth);
            var newStoriesThisMonth = allStories.Count(s => s.CreatedOn >= startOfMonth);

            // Get approved/pending therapists
            var approvedTherapists = allTherapists.Count(t => t.IsAdminApproved);
            var pendingTherapists = allTherapists.Count(t => !t.IsAdminApproved);

            // Get pending flagged content
            var pendingFlaggedContent = allFlaggedContent.Count(fc => !fc.IsReviewed);

            // Get recent users
            var recentUsers = allUsers.OrderByDescending(u => u.CreatedOn).Take(10);

            // Get pending reports
            var pendingReports = allFlaggedContent.Where(fc => !fc.IsReviewed).Take(10);

            response.Data = new AdminDashboardDto
            {
                TotalUsers = allUsers.Count(),
                TotalStories = allStories.Count(),
                TotalComments = allComments.Count(),
                TotalReactions = allReactions.Count(),
                TotalTherapists = allTherapists.Count(),
                ApprovedTherapists = approvedTherapists,
                PendingTherapists = pendingTherapists,
                TotalSessions = allSessions.Count(),
                PendingFlaggedContent = pendingFlaggedContent,
                NewUsersThisMonth = newUsersThisMonth,
                NewStoriesThisMonth = newStoriesThisMonth,
                RecentUsers = recentUsers.Select(u => new UserDto
                {
                    Id = u.Id,
                    Username = u.Username,
                    Role = u.Role,
                    IsActive = u.IsActive,
                    CreatedOn = u.CreatedOn
                }).ToList(),
                PendingReports = pendingReports.Select(fc => new FlaggedContentDto
                {
                    Id = fc.Id,
                    StoryId = fc.StoryId,
                    CommentId = fc.CommentId,
                    ReportedByUserId = fc.ReportedByUserId,
                    Reason = fc.Reason,
                    IsReviewed = fc.IsReviewed,
                    AdminResponse = fc.AdminResponse
                }).ToList()
            };

            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<ReportsDashboardDto>> GetReportsDashboard()
        {
            var response = new BaseResponse<ReportsDashboardDto>();
            var userRole = _validatorHelper.GetUserRole();

            if (userRole != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            // Get all flagged content
            var allFlaggedContent = await _unitOfWork.FlaggedContent.GetAll(fc => !fc.IsDeleted);

            // Get statistics
            var totalFlagged = allFlaggedContent.Count();
            var pendingReview = allFlaggedContent.Count(fc => !fc.IsReviewed);
            var reviewedContent = allFlaggedContent.Count(fc => fc.IsReviewed);
            var flaggedStories = allFlaggedContent.Count(fc => fc.StoryId != Guid.Empty);
            var flaggedComments = allFlaggedContent.Count(fc => fc.CommentId != Guid.Empty);

            // Get deleted content this month (using IsDeleted flag)
            var contentRemovedThisMonth = allFlaggedContent.Count(fc => 
                fc.IsReviewed && 
                fc.LastModifiedOn.HasValue && 
                fc.LastModifiedOn.Value >= startOfMonth);

            // Get recent flags
            var recentFlags = allFlaggedContent.OrderByDescending(fc => fc.CreatedOn).Take(20);

            // Get flag reason breakdown
            var flagReasonBreakdown = allFlaggedContent
                .GroupBy(fc => fc.Reason ?? "Unknown")
                .ToDictionary(g => g.Key, g => g.Count());

            response.Data = new ReportsDashboardDto
            {
                TotalFlaggedContent = totalFlagged,
                PendingReview = pendingReview,
                ReviewedContent = reviewedContent,
                FlaggedStories = flaggedStories,
                FlaggedComments = flaggedComments,
                ContentRemovedThisMonth = contentRemovedThisMonth,
                RecentFlags = recentFlags.Select(fc => new FlaggedContentDto
                {
                    Id = fc.Id,
                    StoryId = fc.StoryId,
                    CommentId = fc.CommentId,
                    ReportedByUserId = fc.ReportedByUserId,
                    Reason = fc.Reason,
                    IsReviewed = fc.IsReviewed,
                    AdminResponse = fc.AdminResponse,
                    FlaggedAt = fc.CreatedOn
                }).ToList(),
                FlagReasonBreakdown = flagReasonBreakdown
            };

            response.Status = true;
            response.Message = "Success";
            return response;
        }
    }
}
