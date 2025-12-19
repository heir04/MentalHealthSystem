using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class TherapistRepository : BaseRepository<Therapist>, ITherapistRepository
    {
        public TherapistRepository(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Therapist> GetTherapist(Expression<Func<Therapist, bool>> expression)
        {
            return await _context.Therapists
                .Where(expression)
                .Include(s => s.User)
                .FirstOrDefaultAsync();
        }
    }
}