using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class FlaggedContentRepository : BaseRepository<FlaggedContent>, IFlaggedContentRepository
    {
        public FlaggedContentRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}