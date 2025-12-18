using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Application.DTOs
{
    public class TherapySessionDto
    {
        public Guid Id { get; set; }
        public string? Email { get; set; }
        public Guid UserId { get; set; }
        public Guid TherapistId { get; set; }
        public string? TherapistName { get; set; }
        public string? Status { get; set; }
    }
}