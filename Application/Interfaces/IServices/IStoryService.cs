using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface IStoryService
    {
        Task<BaseResponse<CreateStoryDto>> Create(CreateStoryDto storyDto);
        Task<BaseResponse<StoryDto>> Delete(Guid id);
        Task<BaseResponse<StoryDto>> Get(Guid id);
        Task<BaseResponse<IEnumerable<StoryDto>>> GetAll();
        Task<BaseResponse<IEnumerable<StoryDto>>> GetAllUserStory();
        Task<BaseResponse<UpdateStoryDto>> Update(Guid id, UpdateStoryDto storyDto);
    }
}