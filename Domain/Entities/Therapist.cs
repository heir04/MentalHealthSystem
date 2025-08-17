using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Domain.Entities
{
    public class Therapist : BaseEntity
    {
        public Guid UserId { get; set; }
        public string? FullName { get; set; } 
        public string? Specialization { get; set; }
        public string? Bio { get; set; }
        public string? ContactLink { get; set; }
        public TherapistAvailability Availability { get; set; }

        public User? User { get; set; }
    }
}