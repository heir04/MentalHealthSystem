using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Application.DTOs
{
    public class TherapistDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? FullName { get; set; }
        public string? Specialization { get; set; }
        public string? CertificationLink { get; set; }
        public string? Bio { get; set; }
        public string? ContactLink { get; set; }
        public TherapistAvailability Availability { get; set; }
        public string? UserName { get; set; } // Optionally include related user info
    }

    public class CreateTherapistDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string? Email { get; set; }
        public string? CertificationLink { get; set; }
        public string? Bio { get; set; }
        public string? ContactLink { get; set; }
    }

    public class UpdateTherapistDto
    {
        public string FullName { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string? Bio { get; set; }
        public string? ContactLink { get; set; }
        public TherapistAvailability Availability { get; set; }
    }
}
