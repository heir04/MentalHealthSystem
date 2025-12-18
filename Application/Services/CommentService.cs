using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class CommentService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper) : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        public async Task<BaseResponse<CreateCommentDto>> Create(CreateCommentDto commentDto, Guid storyId)
        {
            var response = new BaseResponse<CreateCommentDto>();

            var story = await _unitOfWork.Story.Get(s => !s.IsDeleted);
            if (story is null)
            {
                response.Message = "Story not found!";
                return response;
            }

            var comment = new Comment
            {
                StoryId = story.Id,
                UserId = _validatorHelper.GetUserId(),
                Content = commentDto.Content,
            };

            await _unitOfWork.Comment.Register(comment);
            await _unitOfWork.SaveChangesAsync();
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<CommentDto>> Delete(Guid id)
        {
            var response = new BaseResponse<CommentDto>();

            var comment = await _unitOfWork.Comment.Get(c => !c.IsDeleted);
            if (comment is null)
            {
                response.Message = "Not found!";
                return response;
            }
            
            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            if (comment.UserId != userId || _validatorHelper.GetUserRole() != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }


            comment.IsDeleted = true;
            comment.IsDeletedBy = userId;
            comment.IsDeletedOn = DateTime.UtcNow;

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<CommentDto>> Get(Guid id)
        {
            var response = new BaseResponse<CommentDto>();

            var comment = await _unitOfWork.Comment.Get(c => c.Id == id && !c.IsDeleted);
            if (comment is null)
            {
                response.Message = "Not found!";
                return response;
            }

            response.Data = new CommentDto
            {
                Id = comment.Id,
                Content = comment.Content,
                UserId = comment.UserId,
                StoryId = comment.StoryId,
                CreatedOn = comment.CreatedOn
            };
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<CommentDto>> Update(Guid id, UpdateCommentDto commentDto)
        {
            var response = new BaseResponse<CommentDto>();

            var comment = await _unitOfWork.Comment.Get(c => c.Id == id && !c.IsDeleted);
            if (comment is null)
            {
                response.Message = "Not found!";
                return response;
            }

            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            if (comment.UserId != userId && _validatorHelper.GetUserRole() != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }

            comment.Content = commentDto.Content;
            comment.LastModifiedBy = userId;
            comment.LastModifiedOn = DateTime.UtcNow;

            response.Message = "Success";
            response.Status = true;
            return response;
        }
    }
}