using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IFlaggedContentService
    {
        Task<BaseResponse<CreateFlaggedContentDto>> ReportStory(CreateFlaggedContentDto flaggedContentDto, Guid storyId);
        Task<BaseResponse<CreateFlaggedContentDto>> ReportComment(CreateFlaggedContentDto flaggedContentDto, Guid commentId);
        Task<BaseResponse<FlaggedContentDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<FlaggedContentDto>>> GetAll();
        Task<BaseResponse<UpdateFlaggedContentDto>> Review(Guid id, UpdateFlaggedContentDto updateContentDto);
    }
}