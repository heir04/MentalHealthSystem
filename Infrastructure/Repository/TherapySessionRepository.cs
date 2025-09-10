using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class TherapySessionRepository : BaseRepository<TherapySession>, ITherapySessionRepository
    {
        public TherapySessionRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}