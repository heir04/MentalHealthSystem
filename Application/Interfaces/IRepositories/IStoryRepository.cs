using System.Linq.Expressions;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface IStoryRepository : IBaseRepository<Story>
    {
        Task<Story> GetStory(Expression<Func<Story, bool>> expression);
        Task<IEnumerable<Story>> GetAllStory(Expression<Func<Story, bool>> expression);
    }
}