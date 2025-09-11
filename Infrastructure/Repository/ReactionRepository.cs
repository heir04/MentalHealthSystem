using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class ReactionRepository : BaseRepository<Reaction>, IReactionRepository
    {
        public ReactionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Reaction>> GetReactionsByStoryAsync(Guid storyId)
        {
            return await _context.Reactions
                .Include(r => r.User)
                .Where(r => r.StoryId == storyId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reaction>> GetReactionsByCommentAsync(Guid commentId)
        {
            return await _context.Reactions
                .Include(r => r.User)
                .Where(r => r.CommentId == commentId && !r.IsDeleted)
                .ToListAsync();
        }

        public async Task<Reaction?> GetUserReactionAsync(Guid userId, Guid? storyId, Guid? commentId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == userId && 
                                         r.StoryId == storyId && 
                                         r.CommentId == commentId && 
                                         !r.IsDeleted);
        }

        public async Task<Reaction?> GetUserReactionIncludingDeletedAsync(Guid userId, Guid? storyId, Guid? commentId, string reactionType)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.UserId == userId && 
                                         r.StoryId == storyId && 
                                         r.CommentId == commentId && 
                                         r.Type == reactionType);
        }

        public async Task<int> GetReactionCountAsync(Guid? storyId, Guid? commentId, string? type = null)
        {
            var query = _context.Reactions.Where(r => !r.IsDeleted);

            if (storyId.HasValue)
                query = query.Where(r => r.StoryId == storyId);

            if (commentId.HasValue)
                query = query.Where(r => r.CommentId == commentId);

            if (!string.IsNullOrEmpty(type))
                query = query.Where(r => r.Type == type);

            return await query.CountAsync();
        }

        public async Task<IEnumerable<IGrouping<string, Reaction>>> GetReactionGroupsByStoryAsync(Guid storyId)
        {
            return await _context.Reactions
                .Include(r => r.User)
                .Where(r => r.StoryId == storyId && !r.IsDeleted)
                .GroupBy(r => r.Type)
                .ToListAsync();
        }

        public async Task<IEnumerable<IGrouping<string, Reaction>>> GetReactionGroupsByCommentAsync(Guid commentId)
        {
            return await _context.Reactions
                .Include(r => r.User)
                .Where(r => r.CommentId == commentId && !r.IsDeleted)
                .GroupBy(r => r.Type)
                .ToListAsync();
        }
    }
}
