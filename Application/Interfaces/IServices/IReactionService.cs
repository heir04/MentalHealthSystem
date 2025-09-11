using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IReactionService
    {
        Task<BaseResponse<ReactionDto>> ReactToStoryAsync(Guid storyId, CreateReactionDto reactionDto);
        Task<BaseResponse<ReactionDto>> ReactToCommentAsync(Guid commentId, CreateReactionDto reactionDto);
        Task<BaseResponse<bool>> RemoveReactionFromStoryAsync(Guid storyId);
        Task<BaseResponse<bool>> RemoveReactionFromCommentAsync(Guid commentId);
        Task<BaseResponse<IEnumerable<ReactionDto>>> GetStoryReactionsAsync(Guid storyId);
        Task<BaseResponse<IEnumerable<ReactionDto>>> GetCommentReactionsAsync(Guid commentId);
        Task<BaseResponse<IEnumerable<ReactionSummaryDto>>> GetStoryReactionSummaryAsync(Guid storyId);
        Task<BaseResponse<IEnumerable<ReactionSummaryDto>>> GetCommentReactionSummaryAsync(Guid commentId);
    }
}
