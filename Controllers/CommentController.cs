using MentalHealthSystem.Application.DTOs;
using MentalHealthSystem.Application.Interfaces.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MentalHealthSystem.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentController(ICommentService commentService) : ControllerBase
    {
        private readonly ICommentService _commentService = commentService;

        [HttpPost("Create/{storyId}")]
        [Authorize(Roles = "User,Therapist")]
        public async Task<IActionResult> Create([FromRoute] Guid storyId, CreateCommentDto commentDto)
        {
            var response = await _commentService.Create(commentDto, storyId);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPut("Update/{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> Update([FromRoute] Guid id, UpdateCommentDto commentDto)
        {
            var response = await _commentService.Update(id, commentDto);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpPost("Delete/{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            var response = await _commentService.Delete(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }

        [HttpGet("Get/{id}")]
        [Authorize(Roles = "User,Therapist,Admin")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _commentService.Get(id);
            return response.Status ? Ok(response) : BadRequest(response);
        }
    }
}
