
using SecurityDriven;

namespace MentalHealthSystem.Domain.Entities
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = FastGuid.NewGuid();
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime LastModifiedOn { get; set; }
        public Guid LastModifiedBy { get; set; }
        public DateTime IsDeletedOn { get; set; }
        public Guid IsDeletedBy { get; set; }
        public bool IsDeleted { get; set; }
    }
}