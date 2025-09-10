using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Domain.Entities
{
    public class TherapySession : BaseEntity
    {
        public Guid UserId { get; set; }
        public User? User { get; set; }
        public Guid TherapistId { get; set; }
        public Therapist? Therapist { get; set; }
        public TherapySessionStatus Status { get; set; } = TherapySessionStatus.Pending;
    }
}