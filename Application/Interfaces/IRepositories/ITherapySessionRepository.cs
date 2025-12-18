using System.Linq.Expressions;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface ITherapySessionRepository : IBaseRepository<TherapySession>
    {
        Task<IEnumerable<TherapySession>> GetAllSessions(Expression<Func<TherapySession, bool>> expression);
    }
}