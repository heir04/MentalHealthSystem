using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class ReactionService : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ValidatorHelper _validatorHelper;

        public ReactionService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper)
        {
            _unitOfWork = unitOfWork;
            _validatorHelper = validatorHelper;
        }

        public async Task<BaseResponse<ReactionDto>> ReactToStoryAsync(Guid storyId, CreateReactionDto reactionDto)
        {
            var response = new BaseResponse<ReactionDto>();
            var userId = _validatorHelper.GetUserId();

            var story = await _unitOfWork.Story.Get(s => s.Id == storyId && !s.IsDeleted);
            if (story == null)
            {
                response.Message = "Story not found";
                return response;
            }

            var existingReaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, storyId, null);
            
            if (existingReaction != null)
            {
                existingReaction.Type = reactionDto.Type;
                existingReaction.LastModifiedOn = DateTime.UtcNow;
                existingReaction.LastModifiedBy = userId;

                await _unitOfWork.SaveChangesAsync();

                response.Data = new ReactionDto
                {
                    Id = existingReaction.Id,
                    UserId = existingReaction.UserId,
                    StoryId = existingReaction.StoryId,
                    Type = existingReaction.Type,
                    CreatedOn = existingReaction.CreatedOn
                };
                response.Status = true;
                response.Message = "Reaction updated successfully";
                return response;
            }

            var deletedReaction = await _unitOfWork.Reaction.GetUserReactionIncludingDeletedAsync(userId, storyId, null, reactionDto.Type);
            
            if (deletedReaction != null && deletedReaction.IsDeleted)
            {
                // Reactivate the soft-deleted reaction
                deletedReaction.IsDeleted = false;
                deletedReaction.IsDeletedOn = default;
                deletedReaction.IsDeletedBy = default;
                deletedReaction.LastModifiedOn = DateTime.UtcNow;
                deletedReaction.LastModifiedBy = userId;

                await _unitOfWork.SaveChangesAsync();

                response.Data = new ReactionDto
                {
                    Id = deletedReaction.Id,
                    UserId = deletedReaction.UserId,
                    StoryId = deletedReaction.StoryId,
                    Type = deletedReaction.Type,
                    CreatedOn = deletedReaction.CreatedOn
                };
                response.Status = true;
                response.Message = "Reaction added successfully";
                return response;
            }

            // Create new reaction if no existing or deleted reaction found
            var reaction = new Reaction
            {
                UserId = userId,
                StoryId = storyId,
                Type = reactionDto.Type
            };

            await _unitOfWork.Reaction.Register(reaction);
            await _unitOfWork.SaveChangesAsync();

            response.Data = new ReactionDto
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                StoryId = reaction.StoryId,
                Type = reaction.Type
            };
            response.Status = true;
            response.Message = "Reaction added successfully";
            return response;
        }

        public async Task<BaseResponse<ReactionDto>> ReactToCommentAsync(Guid commentId, CreateReactionDto reactionDto)
        {
            var response = new BaseResponse<ReactionDto>();
            var userId = _validatorHelper.GetUserId();

            var comment = await _unitOfWork.Comment.Get(c => c.Id == commentId && !c.IsDeleted);
            if (comment == null)
            {
                response.Message = "Comment not found";
                return response;
            }

            // Check if user already has an active reaction to this comment
            var existingReaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, null, commentId);
            
            if (existingReaction != null)
            {
                existingReaction.Type = reactionDto.Type;
                existingReaction.LastModifiedOn = DateTime.UtcNow;
                existingReaction.LastModifiedBy = userId;
                
                await _unitOfWork.SaveChangesAsync();

                response.Data = new ReactionDto
                {
                    Id = existingReaction.Id,
                    UserId = existingReaction.UserId,
                    CommentId = existingReaction.CommentId,
                    Type = existingReaction.Type,
                };
                response.Status = true;
                response.Message = "Reaction updated successfully";
                return response;
            }

            // Check if user has a soft-deleted reaction of the same type
            var deletedReaction = await _unitOfWork.Reaction.GetUserReactionIncludingDeletedAsync(userId, null, commentId, reactionDto.Type);
            
            if (deletedReaction != null && deletedReaction.IsDeleted)
            {
                // Reactivate the soft-deleted reaction
                deletedReaction.IsDeleted = false;
                deletedReaction.IsDeletedOn = default;
                deletedReaction.IsDeletedBy = default;
                deletedReaction.LastModifiedOn = DateTime.UtcNow;
                deletedReaction.LastModifiedBy = userId;

                await _unitOfWork.SaveChangesAsync();

                response.Data = new ReactionDto
                {
                    Id = deletedReaction.Id,
                    UserId = deletedReaction.UserId,
                    CommentId = deletedReaction.CommentId,
                    Type = deletedReaction.Type,
                    CreatedOn = deletedReaction.CreatedOn
                };
                response.Status = true;
                response.Message = "Reaction added successfully";
                return response;
            }

            // Create new reaction if no existing or deleted reaction found
            var reaction = new Reaction
            {
                UserId = userId,
                CommentId = commentId,
                Type = reactionDto.Type
            };

            await _unitOfWork.Reaction.Register(reaction);
            await _unitOfWork.SaveChangesAsync();

            response.Data = new ReactionDto
            {
                Id = reaction.Id,
                UserId = reaction.UserId,
                CommentId = reaction.CommentId,
                Type = reaction.Type,
                CreatedOn = reaction.CreatedOn
            };
            response.Status = true;
            response.Message = "Reaction added successfully";
            return response;
        }

        public async Task<BaseResponse<bool>> RemoveReactionFromStoryAsync(Guid storyId)
        {
            var response = new BaseResponse<bool>();
            var userId = _validatorHelper.GetUserId();

            var reaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, storyId, null);
            if (reaction == null)
            {
                response.Message = "Reaction not found";
                return response;
            }

            reaction.IsDeleted = true;
            reaction.IsDeletedOn = DateTime.UtcNow;
            reaction.IsDeletedBy = userId;

            await _unitOfWork.SaveChangesAsync();

            response.Data = true;
            response.Status = true;
            response.Message = "Reaction removed successfully";
            return response;
        }

        public async Task<BaseResponse<bool>> RemoveReactionFromCommentAsync(Guid commentId)
        {
            var response = new BaseResponse<bool>();
            var userId = _validatorHelper.GetUserId();

            var reaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, null, commentId);
            if (reaction == null)
            {
                response.Message = "Reaction not found";
                return response;
            }

            reaction.IsDeleted = true;
            reaction.IsDeletedOn = DateTime.UtcNow;
            reaction.IsDeletedBy = userId;

            await _unitOfWork.SaveChangesAsync();

            response.Data = true;
            response.Status = true;
            response.Message = "Reaction removed successfully";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<ReactionDto>>> GetStoryReactionsAsync(Guid storyId)
        {
            var response = new BaseResponse<IEnumerable<ReactionDto>>();

            var reactions = await _unitOfWork.Reaction.GetReactionsByStoryAsync(storyId);

            response.Data = reactions.Select(r => new ReactionDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User?.Username,
                StoryId = r.StoryId,
                Type = r.Type,
                CreatedOn = r.CreatedOn
            });

            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<ReactionDto>>> GetCommentReactionsAsync(Guid commentId)
        {
            var response = new BaseResponse<IEnumerable<ReactionDto>>();

            var reactions = await _unitOfWork.Reaction.GetReactionsByCommentAsync(commentId);

            response.Data = reactions.Select(r => new ReactionDto
            {
                Id = r.Id,
                UserId = r.UserId,
                UserName = r.User?.Username,
                CommentId = r.CommentId,
                Type = r.Type,
                CreatedOn = r.CreatedOn
            });

            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<ReactionSummaryDto>>> GetStoryReactionSummaryAsync(Guid storyId)
        {
            var response = new BaseResponse<IEnumerable<ReactionSummaryDto>>();
            var userId = _validatorHelper.GetUserId();

            var reactionGroups = await _unitOfWork.Reaction.GetReactionGroupsByStoryAsync(storyId);
            var userReaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, storyId, null);

            response.Data = reactionGroups.Select(g => new ReactionSummaryDto
            {
                Type = g.Key,
                Count = g.Count(),
                UserReacted = userReaction != null && userReaction.Type == g.Key
            });

            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<ReactionSummaryDto>>> GetCommentReactionSummaryAsync(Guid commentId)
        {
            var response = new BaseResponse<IEnumerable<ReactionSummaryDto>>();
            var userId = _validatorHelper.GetUserId();

            var reactionGroups = await _unitOfWork.Reaction.GetReactionGroupsByCommentAsync(commentId);
            var userReaction = await _unitOfWork.Reaction.GetUserReactionAsync(userId, null, commentId);

            response.Data = reactionGroups.Select(g => new ReactionSummaryDto
            {
                Type = g.Key,
                Count = g.Count(),
                UserReacted = userReaction != null && userReaction.Type == g.Key
            });

            response.Status = true;
            response.Message = "Success";
            return response;
        }
    }
}
