using MentalHealthSystem.Domain.Entities;
using System.Linq.Expressions;

namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface IReactionRepository : IBaseRepository<Reaction>
    {
        Task<IEnumerable<Reaction>> GetReactionsByStoryAsync(Guid storyId);
        Task<IEnumerable<Reaction>> GetReactionsByCommentAsync(Guid commentId);
        Task<Reaction?> GetUserReactionAsync(Guid userId, Guid? storyId, Guid? commentId);
        Task<Reaction?> GetUserReactionIncludingDeletedAsync(Guid userId, Guid? storyId, Guid? commentId, string reactionType);
        Task<int> GetReactionCountAsync(Guid? storyId, Guid? commentId, string? type = null);
        Task<IEnumerable<IGrouping<string, Reaction>>> GetReactionGroupsByStoryAsync(Guid storyId);
        Task<IEnumerable<IGrouping<string, Reaction>>> GetReactionGroupsByCommentAsync(Guid commentId);
    }
}
