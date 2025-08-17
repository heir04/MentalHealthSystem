using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Domain.Entities;
using MentalHealthSystem.Infrastructure.Data;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationContext context)
        {
            _context = context;
        }
    }
}