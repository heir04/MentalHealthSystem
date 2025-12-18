using System.Linq.Expressions;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class TherapySessionRepository : BaseRepository<TherapySession>, ITherapySessionRepository
    {
        public TherapySessionRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TherapySession>> GetAllSessions(Expression<Func<TherapySession, bool>> expression)
        {
            return await _context.TherapySessions
                .Where(expression)
                .Include(ts => ts.User)
                .Include(ts => ts.Therapist)
                .ToListAsync();
        }
    }
}