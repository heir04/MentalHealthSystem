using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IFlaggedContentService
    {
        Task<BaseResponse<CreateFlaggedContentDto>> ReportStory(CreateFlaggedContentDto flaggedContentDto, Guid StoryId);
        Task<BaseResponse<CreateFlaggedContentDto>> ReportComment(CreateFlaggedContentDto flaggedContentDto, Guid StoryId);
        Task<BaseResponse<FlaggedContentDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<FlaggedContentDto>>> GetAll();
        Task<BaseResponse<UpdateFlaggedContentDto>> Review(Guid id, UpdateFlaggedContentDto updateContentDto);
    }
}