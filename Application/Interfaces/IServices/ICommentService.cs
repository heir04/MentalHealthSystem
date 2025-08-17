using MentalHealthSystem.Application.DTOs;

namespace MentalHealthSystem.Application.Interfaces.IServices
{
    public interface ICommentService
    {
        Task<BaseResponse<CreateCommentDto>> Create(CreateCommentDto commentDto, Guid storyId);
        Task<BaseResponse<CommentDto>> Delete(Guid id);
        Task<BaseResponse<CommentDto>> Get(Guid id);
        Task<BaseResponse<CommentDto>> Update(Guid id, UpdateCommentDto commentDto);
    }
}