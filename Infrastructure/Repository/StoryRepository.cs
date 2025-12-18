using System.Linq.Expressions;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class StoryRepository : BaseRepository<Story>, IStoryRepository
    {
        public StoryRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Story>> GetAllStory(Expression<Func<Story, bool>> expression)
        {
            return await _context.Story
                .Where(expression)
                .Include(s => s.User)
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .ToListAsync();
        }

        public async Task<Story> GetStory(Expression<Func<Story, bool>> expression)
        {
            return await _context.Story
                .Where(expression)
                .Include(s => s.User)
                .Include(s => s.Comments)
                .ThenInclude(c => c.User)
                .FirstOrDefaultAsync();
        }
    }
}