using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Domain.Enums;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface ITherapySessionService
    {
        Task<BaseResponse<TherapySessionDto>> BookSession(Guid therapistid);
        Task<BaseResponse<TherapySessionDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<TherapySessionDto>>> GetAllByTherapist();
        Task<BaseResponse<IEnumerable<TherapySessionDto>>> GetAllByActiveUser();
        Task<BaseResponse<TherapySessionDto>> UpdateSessionStatus(Guid id, TherapySessionStatus status);
    }
}