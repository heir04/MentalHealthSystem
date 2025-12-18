using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoryController(IStoryService storyService) : ControllerBase
    {
        private readonly IStoryService _storyService = storyService;

        [HttpPost("Create")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Create(CreateStoryDto storyDto)
        {
            var response = await _storyService.Create(storyDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateStoryDto storyDto)
        {
            var response = await _storyService.Update(id, storyDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Delete/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _storyService.Delete(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "User,Therapist,Admin")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _storyService.GetAll();
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get/{id}")]
        [Authorize(Roles = "User,Therapist,Admin")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _storyService.Get(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("GetAllUserStory")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetAllUserStory()
        {
            var response = await _storyService.GetAllUserStory();
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
