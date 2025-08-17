using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class TherapistRepository : BaseRepository<Therapist>, ITherapistRepository
    {
        public TherapistRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}