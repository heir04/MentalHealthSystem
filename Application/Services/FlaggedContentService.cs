using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class FlaggedContentService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper) : IFlaggedContentService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        public async Task<BaseResponse<FlaggedContentDto>> Get(Guid id)
        {
            var response = new BaseResponse<FlaggedContentDto>();
            var flaggedContent = await _unitOfWork.FlaggedContent.Get(f => f.Id == id && !f.IsDeleted);
            if (flaggedContent is null)
            {
                response.Message = "Flagged content not found";
                return response;
            }

            response.Data = new FlaggedContentDto
            {
                Id = flaggedContent.Id,
                ReportedByUserId = flaggedContent.ReportedByUserId,
                StoryId = flaggedContent.StoryId,
                Reason = flaggedContent.Reason,
                IsReviewed = flaggedContent.IsReviewed,
                AdminResponse = flaggedContent.AdminResponse
            };
            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<IEnumerable<FlaggedContentDto>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<FlaggedContentDto>>();
            var flaggedContents = await _unitOfWork.FlaggedContent.GetAll(f => !f.IsDeleted );
            if (flaggedContents is null || !flaggedContents.Any())
            {
                response.Message = "No flagged content found";
                return response;
            }

            response.Data = flaggedContents.Select(f => new FlaggedContentDto
            {
                Id = f.Id,
                ReportedByUserId = f.ReportedByUserId,
                StoryId = f.StoryId,
                Reason = f.Reason,
                IsReviewed = f.IsReviewed,
                AdminResponse = f.AdminResponse
            });
            response.Status = true;
            response.Message = "Success";
            return response;
        }

        public async Task<BaseResponse<CreateFlaggedContentDto>> ReportComment(CreateFlaggedContentDto flaggedContentDto, Guid commentId)
        {
            var response = new BaseResponse<CreateFlaggedContentDto>();
            var commentExists = await _unitOfWork.Story.ExistsAsync(s => s.Id == commentId && !s.IsDeleted);
            if (!commentExists)
            {
                response.Message = "Comment not found";
                return response;
            }

            var userId = _validatorHelper.GetUserId();

            var flagExist = await _unitOfWork.FlaggedContent.ExistsAsync(fc => fc.CommentId == commentId && fc.ReportedByUserId == userId);
            if (flagExist)
            {
                response.Message = "Comments already reported by you";
                return response;
            }

            var flaggedContent = new FlaggedContent
            {
                CommentId = flaggedContentDto.CommentId,
                ReportedByUserId = userId,
                Reason = flaggedContentDto.Reason
            };

            await _unitOfWork.FlaggedContent.Register(flaggedContent);
            await _unitOfWork.SaveChangesAsync();

            response.Data = flaggedContentDto;
            response.Status = true;
            response.Message = "Comment reported successfully";
            return response;
        }

        public async Task<BaseResponse<CreateFlaggedContentDto>> ReportStory(CreateFlaggedContentDto flaggedContentDto, Guid storyId)
        {
            var response = new BaseResponse<CreateFlaggedContentDto>();
            var storyExists = await _unitOfWork.Story.ExistsAsync(s => s.Id == storyId && !s.IsDeleted);
            if (!storyExists)
            {
                response.Message = "Story not found";
                return response;
            }

            var userId = _validatorHelper.GetUserId();

            var flagExist = await _unitOfWork.FlaggedContent.ExistsAsync(fc => fc.StoryId == storyId && fc.ReportedByUserId == userId);
            if (flagExist)
            {
                response.Message = "Comments already reported by you";
                return response;
            }

            var flaggedContent = new FlaggedContent
            {
                StoryId = storyId,
                ReportedByUserId = userId,
                Reason = flaggedContentDto.Reason
            };

            await _unitOfWork.FlaggedContent.Register(flaggedContent);
            await _unitOfWork.SaveChangesAsync();

            response.Data = flaggedContentDto;
            response.Status = true;
            response.Message = "Story reported successfully";
            return response;
        }

        public async Task<BaseResponse<UpdateFlaggedContentDto>> Review(Guid id, UpdateFlaggedContentDto updateContentDto)
        {
            var response = new BaseResponse<UpdateFlaggedContentDto>();
            var flaggedContent = await _unitOfWork.FlaggedContent.Get(f => f.Id == id && !f.IsDeleted);
            if (flaggedContent is null)
            {
                response.Message = "Flagged content not found";
                return response;
            }

            flaggedContent.AdminResponse = updateContentDto.AdminResponse;
            flaggedContent.IsReviewed = true;

            await _unitOfWork.SaveChangesAsync();

            response.Status = true;
            response.Message = "Flagged content reviewed successfully";
            return response;
        }
    }
}