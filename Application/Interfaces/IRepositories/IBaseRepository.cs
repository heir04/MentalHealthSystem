using System.Linq.Expressions;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface IBaseRepository<T> where T : BaseEntity, new()
    {
        Task<T> Register(T entity);
        Task<T> Update(T entity);
        Task<T> GetAsync(Guid id);
        Task<int> Count(Expression<Func<T, bool>> expression);
        Task<T> Get(Expression<Func<T, bool>> expression);
        Task<IList<T>> GetAll(Expression<Func<T, bool>> expression);
        Task<IList<T>> GetAll();
        Task<List<T>> GetAllByIdsAsync(List<Guid> ids);
        Task<bool> ExistsAsync(Expression<Func<T, bool>> expression);
        Task<IList<T>> GetByExpression(Expression<Func<T, bool>> expression);
    }
}