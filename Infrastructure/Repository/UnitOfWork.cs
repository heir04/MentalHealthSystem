using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Infrastructure.Data;

namespace MentalHealthSystem.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private bool _disposed = false;
        public ICommentRepository Comment { get; }
        public IFlaggedContentRepository FlaggedContent { get; }
        public IStoryRepository Story { get; }
        public ITherapistRepository Therapist { get; }
        public IUserRepository User { get; }

        public UnitOfWork(
            ApplicationContext context,
            ICommentRepository commentRepository,
            IFlaggedContentRepository flaggedContentRepository,
            IStoryRepository storyRepository,
            ITherapistRepository therapistRepository,
            IUserRepository userRepository
        )
        {
            _context = context;
            Comment = commentRepository;
            FlaggedContent = flaggedContentRepository;
            Story = storyRepository;
            Therapist = therapistRepository;
            User = userRepository;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
                _disposed = true;
            }
        }
    }
}