namespace MentalHealthSystem.Application.Interfaces.IRepositories
{
    public interface IUnitOfWork
    {
        ICommentRepository Comment { get; }
        IFlaggedContentRepository FlaggedContent { get; }
        IReactionRepository Reaction { get; }
        IStoryRepository Story { get; }
        ITherapistRepository Therapist { get; }
        ITherapySessionRepository TherapySession { get; }
        IUserRepository User { get; }
        Task<int> SaveChangesAsync();
    }
}