using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Helpers;
using MentalHealthSystem.Application.Interfaces.IRepositories;
using MentalHealthSystem.Application.Interfaces.IServices;
using MentalHealthSystem.Domain.Entities;

namespace MentalHealthSystem.Application.Services
{
    public class StoryService(IUnitOfWork unitOfWork, ValidatorHelper validatorHelper) : IStoryService
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly ValidatorHelper _validatorHelper = validatorHelper;
        public async Task<BaseResponse<CreateStoryDto>> Create(CreateStoryDto storyDto)
        {
            var response = new BaseResponse<CreateStoryDto>();

            var story = new Story
            {
                Content = storyDto.Content,
                UserId = _validatorHelper.GetUserId(),
            };

            await _unitOfWork.Story.Register(story);
            await _unitOfWork.SaveChangesAsync();
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<StoryDto>> Delete(Guid id)
        {
            var response = new BaseResponse<StoryDto>();
            var story = await _unitOfWork.Story.Get(s => s.Id == id && !s.IsDeleted);
            if (story is null)
            {
                response.Message = "Not found";
                return response;
            }

            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            if (userId != story.UserId && _validatorHelper.GetUserRole() != "Admin")
            {
                response.Message = "Not Authorized";
                return response;
            }

            story.IsDeleted = true;
            story.IsDeletedOn = DateTime.UtcNow;
            story.IsDeletedBy = userId;

            await _unitOfWork.SaveChangesAsync();

            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<StoryDto>> Get(Guid id)
        {
            var response = new BaseResponse<StoryDto>();
            var story = await _unitOfWork.Story.GetStory(s => s.Id == id && !s.IsDeleted);
            if (story is null)
            {
                response.Message = "Not Found";
                return response;
            }

            var storyReactionCount = await _unitOfWork.Reaction.GetReactionCountAsync(story.Id, null);

            var commentDtos = new List<CommentDto>();
            foreach (var comment in story.Comments.Where(c => !c.IsDeleted))
            {
                var commentReactionCount = await _unitOfWork.Reaction.GetReactionCountAsync(null, comment.Id);
                commentDtos.Add(new CommentDto
                {
                    Id = comment.Id,
                    Content = comment.Content,
                    StoryId = comment.StoryId,
                    UserId = comment.UserId,
                    LikesCount = commentReactionCount,
                    UserName = "Anonymous",
                    CreatedOn = comment.CreatedOn
                });
            }

            response.Data = new StoryDto
            {
                Id = story.Id,
                UserId = story.UserId,
                UserName = "Anonymous",
                Content = story.Content,
                CommentsCount = story.Comments?.Count(c => !c.IsDeleted) ?? 0,
                LikesCount = storyReactionCount,
                CreatedOn = story.CreatedOn,
                Comments = commentDtos
            };
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<StoryDto>>> GetAll()
        {
            var response = new BaseResponse<IEnumerable<StoryDto>>();
            var stories = await _unitOfWork.Story.GetAllStory(s => !s.IsDeleted);
            var allReactions = await _unitOfWork.Reaction.GetAll(r => !r.IsDeleted);

            if (stories is null || !stories.Any())
            {
                response.Message = "No story found!";
                return response;
            }

            var storyDtos = new List<StoryDto>();
            foreach (var story in stories)
            {
                var storyReactionCount = allReactions.Count(r => r.StoryId == story.Id && !r.IsDeleted);

                storyDtos.Add(new StoryDto
                {
                    Id = story.Id,
                    UserId = story.UserId,
                    UserName = "Anonymous",
                    Content = story.Content,
                    CommentsCount = story.Comments?.Count(c => !c.IsDeleted) ?? 0,
                    LikesCount = storyReactionCount,
                    CreatedOn = story.CreatedOn,
                });
            }

            response.Data = storyDtos;
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<IEnumerable<StoryDto>>> GetAllUserStory()
        {
            var response = new BaseResponse<IEnumerable<StoryDto>>();
            var userId = _validatorHelper.GetUserId();
            if (userId == Guid.Empty)
            {
                response.Message = "User not found";
                return response;
            }

            var stories = await _unitOfWork.Story.GetAllStory(s => !s.IsDeleted && s.UserId == userId);

            if (stories is null || !stories.Any())
            {
                response.Message = "No story found!";
                return response;
            }

            var storyDtos = new List<StoryDto>();
            foreach (var story in stories)
            {
                var storyReactionCount = await _unitOfWork.Reaction.GetReactionCountAsync(story.Id, null);

                var commentDtos = new List<CommentDto>();
                foreach (var comment in story.Comments.Where(c => !c.IsDeleted))
                {
                    var commentReactionCount = await _unitOfWork.Reaction.GetReactionCountAsync(null, comment.Id);
                    commentDtos.Add(new CommentDto
                    {
                        Id = comment.Id,
                        Content = comment.Content,
                        StoryId = comment.StoryId,
                        UserId = comment.UserId,
                        LikesCount = commentReactionCount,
                        UserName = comment.User?.Username,
                        CreatedOn = comment.CreatedOn
                    });
                }

                storyDtos.Add(new StoryDto
                {
                    Id = story.Id,
                    UserId = story.UserId,
                    UserName = story.User?.Username,
                    Content = story.Content,
                    CommentsCount = story.Comments?.Count(c => !c.IsDeleted) ?? 0,
                    LikesCount = storyReactionCount,
                    CreatedOn = story.CreatedOn,
                    Comments = commentDtos
                });
            }

            response.Data = storyDtos;
            response.Message = "Success";
            response.Status = true;
            return response;
        }

        public async Task<BaseResponse<UpdateStoryDto>> Update(Guid id, UpdateStoryDto storyDto)
        {
            var response = new BaseResponse<UpdateStoryDto>();

            var story = await _unitOfWork.Story.Get(s => s.Id == id && !s.IsDeleted);
            if (story is null)
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

            if (userId != story.UserId)
            {
                response.Message = "Not Authorized";
                return response;
            }

            story.Content = storyDto.Content;
            story.LastModifiedOn = DateTime.UtcNow;
            story.LastModifiedBy = _validatorHelper.GetUserId();

            await _unitOfWork.SaveChangesAsync();
            response.Message = "Success";
            response.Status = true;
            return response;
        }
    }
}