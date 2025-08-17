namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        ICommentRepository Comment { get; }
        IFlaggedContentRepository FlaggedContent { get; }
        IStoryRepository Story { get; }
        ITherapistRepository Therapist { get; }
        IUserRepository User { get; }
        Task<int> SaveChangesAsync();
    }
}