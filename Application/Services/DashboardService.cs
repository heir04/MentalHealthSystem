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

            var recentStories = await _unitOfWork.Story.GetAllStory(s => s.UserId == userId && !s.IsDeleted);
            var comments = await _unitOfWork.Comment.GetAll(c => c.UserId == userId && !c.IsDeleted);

            var userStoryIds = recentStories.Select(s => s.Id).ToList();
            var allReactions = await _unitOfWork.Reaction.GetAll(r => 
                userStoryIds.Contains(r.StoryId ?? Guid.Empty) && !r.IsDeleted);
            var reactionsReceived = allReactions.Count();

            var upcomingSessions = await _unitOfWork.TherapySession.GetAll(ts => 
                ts.UserId == userId && 
                (ts.Status == TherapySessionStatus.Pending || ts.Status == TherapySessionStatus.Scheduled));

            response.Data = new UserDashboardDto
            {
                User = new UserDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    Username = user.Username,
                    Role = user.Role,
                    CreatedOn = user.CreatedOn
                },
                TotalStories = recentStories.Count(),
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

        public async Task<BaseResponse<TherapistDashboardDto>> GetTherapistDashboard()
        {
            var response = new BaseResponse<TherapistDashboardDto>();
            var userId = _validatorHelper.GetUserId();

            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            var therapist = await _unitOfWork.Therapist.GetTherapist(t => t.UserId == userId && !t.IsDeleted);
            if (therapist == null)
            {
                response.Message = "Therapist not found";
                return response;
            }

            var startOfMonth = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);

            var allSessions = await _unitOfWork.TherapySession.GetAll(ts => ts.TherapistId == therapist.Id && !ts.IsDeleted);

            var pendingSessions = allSessions.Count(ts => ts.Status == TherapySessionStatus.Pending);
            var scheduledSessions = allSessions.Count(ts => ts.Status == TherapySessionStatus.Scheduled);
            var completedSessions = allSessions.Count(ts => ts.Status == TherapySessionStatus.Completed);
            var cancelledSessions = allSessions.Count(ts => ts.Status == TherapySessionStatus.Cancelled);

            var sessionsThisMonth = allSessions.Count(ts => ts.CreatedOn >= startOfMonth);

            var uniqueClientIds = allSessions.Select(ts => ts.UserId).Distinct().Count();

            var upcomingSessions = allSessions
                .Where(ts => ts.Status == TherapySessionStatus.Pending || ts.Status == TherapySessionStatus.Scheduled)
                .OrderBy(ts => ts.CreatedOn)
                .Take(10);

            var recentSessions = allSessions
                .OrderByDescending(ts => ts.CreatedOn)
                .Take(10);

            response.Data = new TherapistDashboardDto
            {
                Therapist = new TherapistDto
                {
                    Id = therapist.Id,
                    UserId = therapist.UserId,
                    FullName = therapist.FullName,
                    Email = therapist.User.Email,
                    Specialization = therapist.Specialization,
                    CertificationLink = therapist.CertificationLink,
                    Bio = therapist.Bio,
                    ContactLink = therapist.ContactLink,
                    Availability = therapist.Availability,
                    UserName = therapist.User?.Username,
                    Role = therapist.User.Role
                },
                TotalSessions = allSessions.Count(),
                PendingSessions = pendingSessions,
                ScheduledSessions = scheduledSessions,
                CompletedSessions = completedSessions,
                CancelledSessions = cancelledSessions,
                TotalClients = uniqueClientIds,
                SessionsThisMonth = sessionsThisMonth,
                UpcomingSessions = upcomingSessions.Select(ts => new TherapySessionDto
                {
                    Id = ts.Id,
                    UserId = ts.UserId,
                    TherapistId = ts.TherapistId,
                    Status = ts.Status.ToString()
                }).ToList(),
                RecentSessions = recentSessions.Select(ts => new TherapySessionDto
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

            var allFlaggedContent = await _unitOfWork.FlaggedContent.GetAll(fc => !fc.IsDeleted);

            var totalFlagged = allFlaggedContent.Count();
            var pendingReview = allFlaggedContent.Count(fc => !fc.IsReviewed);
            var reviewedContent = allFlaggedContent.Count(fc => fc.IsReviewed);
            var flaggedStories = allFlaggedContent.Count(fc => fc.StoryId != Guid.Empty);
            var flaggedComments = allFlaggedContent.Count(fc => fc.CommentId != Guid.Empty);

            var contentRemovedThisMonth = allFlaggedContent.Count(fc =>
                fc.IsReviewed &&
                fc.LastModifiedOn >= startOfMonth);

            var recentFlags = allFlaggedContent.OrderByDescending(fc => fc.CreatedOn).Take(20);

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

        public async Task<BaseResponse<GeneralDashboardDto>> GetGeneralDashboard()
        {
            var response = new BaseResponse<GeneralDashboardDto>();

            var allStories = await _unitOfWork.Story.GetAllStory(s => !s.IsDeleted);
            var allComments = await _unitOfWork.Comment.GetAll(c => !c.IsDeleted);
            var allUsers = await _unitOfWork.User.GetAll(u => !u.IsDeleted);
            
            // Fetch all reactions once instead of per story/comment
            var allReactions = await _unitOfWork.Reaction.GetAll(r => !r.IsDeleted);
            var totalReactions = allReactions.Count();

            // Group reactions by story and comment in memory
            var reactionsByStory = allReactions
                .Where(r => r.StoryId.HasValue)
                .GroupBy(r => r.StoryId.Value)
                .ToDictionary(g => g.Key, g => g.ToList());

            var reactionsByComment = allReactions
                .Where(r => r.CommentId.HasValue)
                .GroupBy(r => r.CommentId.Value)
                .ToDictionary(g => g.Key, g => g.Count());

            var storiesWithStats = new List<StoryWithStatsDto>();

            foreach (var story in allStories)
            {
                var storyComments = allComments.Where(c => c.StoryId == story.Id && !c.IsDeleted).ToList();

                // Get reactions from in-memory dictionary
                var storyReactions = reactionsByStory.ContainsKey(story.Id) 
                    ? reactionsByStory[story.Id] 
                    : new List<Domain.Entities.Reaction>();

                var storyReactionCount = storyReactions.Count;
                var reactionBreakdown = storyReactions
                    .GroupBy(r => r.Type)
                    .ToDictionary(g => g.Key, g => g.Count());

                var commentDtos = new List<CommentDto>();
                foreach (var comment in storyComments.OrderByDescending(c => c.CreatedOn))
                {
                    // Get comment reaction count from in-memory dictionary
                    var commentReactionCount = reactionsByComment.ContainsKey(comment.Id) 
                        ? reactionsByComment[comment.Id] 
                        : 0;

                    commentDtos.Add(new CommentDto
                    {
                        Id = comment.Id,
                        StoryId = comment.StoryId,
                        UserId = comment.UserId,
                        UserName = comment.User?.Username,
                        Content = comment.Content,
                        LikesCount = commentReactionCount,
                        CreatedOn = comment.CreatedOn
                    });
                }

                storiesWithStats.Add(new StoryWithStatsDto
                {
                    Id = story.Id,
                    UserId = story.UserId,
                    UserName = story.User?.Username,
                    Content = story.Content,
                    CreatedOn = story.CreatedOn,
                    CommentCount = storyComments.Count,
                    ReactionCount = storyReactionCount,
                    Comments = commentDtos,
                    ReactionBreakdown = reactionBreakdown.Any() ? reactionBreakdown : null
                });
            }

            storiesWithStats = storiesWithStats.OrderByDescending(s => s.CreatedOn).ToList();

            response.Data = new GeneralDashboardDto
            {
                TotalStories = allStories.Count(),
                TotalComments = allComments.Count(),
                TotalReactions = totalReactions,
                TotalUsers = allUsers.Count(),
                Stories = storiesWithStats
            };

            response.Status = true;
            response.Message = "Success";
            return response;
        }
    }
}
