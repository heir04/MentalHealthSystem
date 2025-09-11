using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface ITherapistService
    {
        Task<BaseResponse<CreateTherapistDto>> Create(CreateTherapistDto therapistDto);
        Task<BaseResponse<TherapistDto>> Delete(Guid id);
        Task<BaseResponse<TherapistDto>> ApproveTherapist(Guid id);
        Task<BaseResponse<TherapistDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<TherapistDto>>> GetAll();
        Task<BaseResponse<TherapistDto>> Update(Guid id, UpdateTherapistDto therapistDto);
    }
}