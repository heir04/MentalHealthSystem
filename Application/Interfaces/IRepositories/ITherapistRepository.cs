using MentalHealthSystem.Domain.Entities;
using System.Linq.Expressions;

namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface ITherapistRepository : IBaseRepository<Therapist>
    {
        Task<Therapist> GetTherapist(Expression<Func<Therapist, bool>> expression);
    }
}